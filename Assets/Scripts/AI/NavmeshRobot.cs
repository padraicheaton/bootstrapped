using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshRobot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private TrailRenderer jumpTrail;
    [SerializeField] private ParticleSystem chargeFX;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Movement Params")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float spreadRange;
    private Vector3 spreadTargetOffset;
    private float speed;

    [Header("Attack Params")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDmg;
    [SerializeField] private float projectileSpeed;
    private float damage;
    private Coroutine attackRoutineRef;
    private Coroutine returnControlRoutineRef;

    [Header("Misc Params")]
    [SerializeField] private float fireKnockbackForce;
    [SerializeField] private int itemsToDrop;

    public enum State
    {
        Chase,
        Attack,
        Stun,
        Dead,
        Hacked
    }
    private State currentState = State.Chase;

    private Transform target;
    private NavMeshAgent agent;
    private HealthComponent hc;
    private Rigidbody rb;
    private Vector3 wanderTargetDestination;
    private bool canReceiveKnockDamage = false;

    private string[] attackAnimTagOptions = { "Left", "Right", "Both" };
    private string attackAnimTag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = movementSpeed;
        damage = attackDmg;

        target = ServiceLocator.instance.GetService<PlayerMovement>().transform;

        hc = GetComponent<HealthComponent>();
        hc.onDeath += () =>
        {
            SwitchState(State.Dead);
        };

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        SwitchState(State.Chase);
    }

    private void FixedUpdate()
    {
        agent.speed = speed;

        switch (currentState)
        {
            case State.Chase:
            default:
                ChaseTick();
                break;
            case State.Attack:
                AttackTick();
                break;
            case State.Stun:
                StunTick();
                break;
            case State.Dead:
                DeadTick();
                break;
            case State.Hacked:
                HackedTick();
                break;
        }
    }

    private void Update()
    {
        UpdateAnims();
    }

    private void UpdateAnims()
    {
        Vector3 transformedVelocity = transform.InverseTransformVector(agent.desiredVelocity);

        anim.SetFloat("Forward", transformedVelocity.z > 0.1f ? 1f : 0f);

        jumpTrail.emitting = agent.isOnOffMeshLink;
    }

    public void SwitchState(State nextState)
    {
        // EXIT
        if (currentState == State.Stun)
        {
            anim.SetBool("Defend", false);
        }
        else if (currentState == State.Attack)
        {
            if (attackRoutineRef != null)
                StopCoroutine(attackRoutineRef);
            anim.SetBool($"{attackAnimTag} Aim", false);
            chargeFX.Stop();
        }

        // ENTER
        if (nextState == State.Chase)
        {
            SafeSetStopState(false);

            spreadTargetOffset = Random.onUnitSphere * spreadRange;
            spreadTargetOffset.y = 0f;
        }
        else if (nextState == State.Attack)
        {
            SafeSetStopState(true);

            attackAnimTag = attackAnimTagOptions[Random.Range(0, attackAnimTagOptions.Length)];
            anim.SetBool($"{attackAnimTag} Aim", true);

            if (attackRoutineRef != null)
                StopCoroutine(attackRoutineRef);

            attackRoutineRef = StartCoroutine(AttackRoutine());
        }
        else if (nextState == State.Stun)
        {
            SafeSetStopState(true);
            anim.SetBool("Defend", true);
        }
        else if (nextState == State.Dead)
        {
            SafeSetStopState(true);
            anim.SetBool($"{attackAnimTag} Aim", false);
            anim.SetBool("Defend", false);
            anim.SetTrigger("Die");

            OnDeath();
        }
        else if (nextState == State.Hacked)
        {
            agent.isStopped = false;
            NavMesh.SamplePosition(transform.position + Random.onUnitSphere * 2f, out NavMeshHit hit, 10f, NavMesh.AllAreas);
            wanderTargetDestination = hit.position;
        }

        currentState = nextState;
    }

    private void SafeSetStopState(bool stopped)
    {
        if (agent.enabled)
            agent.isStopped = stopped;
    }

    private void ChaseTick()
    {
        if (NavMesh.SamplePosition(target.position + spreadTargetOffset, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        if (Vector3.Distance(transform.position, target.position) <= attackRange)
            //if (!Physics.Raycast(firePoint.position, target.position, attackRange, whatIsGround))
            SwitchState(State.Attack);
    }

    private void HackedTick()
    {
        agent.SetDestination(wanderTargetDestination);

        if (Vector3.Distance(transform.position, wanderTargetDestination) <= 0.1f)
        {
            NavMesh.SamplePosition(transform.position + Random.onUnitSphere * 2f, out NavMeshHit hit, 10f, NavMesh.AllAreas);
            wanderTargetDestination = hit.position;
        }
    }


    private void AttackTick()
    {
        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);

        // Check for tolerance threshold with attack range
        if (Vector3.Distance(transform.position, target.position) > attackRange + 1f)
        {
            SwitchState(State.Chase);
        }
    }

    private void StunTick()
    {

    }

    private void DeadTick()
    {

    }

    private void OnDeath()
    {
        if (itemsToDrop > 0)
            ServiceLocator.instance.GetService<LootController>().OnEnemyDefeated(transform.position, itemsToDrop);

        Destroy(gameObject, 1.5f);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay / 2f);

            chargeFX.Play();

            yield return new WaitForSeconds(attackDelay / 2f);

            chargeFX.Stop();

            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        anim.SetTrigger($"{attackAnimTag} Blast Attack");

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        if (projectile.TryGetComponent<NavmeshRobotProjectile>(out NavmeshRobotProjectile proj))
        {
            proj.Fire(damage, projectileSpeed, target);

            if (fireKnockbackForce > 0f)
                KnockAgent((transform.forward * -1f + Vector3.up * 0.5f) * fireKnockbackForce);
        }
    }

    public void KnockAgent(Vector3 force)
    {
        if (currentState != State.Stun)
        {
            // Only need to do this if the agent is currently not being knocked around
            SwitchState(State.Stun);

            agent.enabled = false;
            rb.isKinematic = false;
        }

        rb.AddForce(force, ForceMode.Impulse);

        if (returnControlRoutineRef != null)
            StopCoroutine(returnControlRoutineRef);

        returnControlRoutineRef = StartCoroutine(ReturnControlFromKnock());
    }

    private IEnumerator ReturnControlFromKnock()
    {
        // Bandaid fix, this delay allows the rigidbody to accelerate before being exited out of
        yield return new WaitForSeconds(0.5f);

        canReceiveKnockDamage = true;

        yield return new WaitWhile(() => rb.velocity.magnitude > 0.5f);

        // Find closest location on Navmesh
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas);
        Vector3 closestPosition = hit.position;

        // Move to that position - this could be changed to move progressively
        transform.position = closestPosition;

        agent.enabled = true;
        rb.isKinematic = true;

        canReceiveKnockDamage = false;

        SwitchState(State.Chase);
    }

    public void SetSlowedState(bool slowed)
    {
        if (slowed)
            speed = movementSpeed / 2f;
        else
            speed = movementSpeed;
    }

    public void SetJammedState(bool jammed)
    {
        if (jammed)
            damage = attackDmg / 2f;
        else
            damage = attackDmg;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (currentState == State.Stun && !rb.isKinematic && canReceiveKnockDamage)
        {
            // If being controlled by physics (i.e. knocked), inflict damage when impacting at high speed
            hc.TakeDamage(rb.velocity.magnitude);
            canReceiveKnockDamage = false;
        }
    }
}
