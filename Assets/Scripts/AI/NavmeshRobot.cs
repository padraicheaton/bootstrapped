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

    [Header("Attack Params")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDmg;
    [SerializeField] private float projectileSpeed;
    private Coroutine attackRoutineRef;

    public enum State
    {
        Chase,
        Attack,
        Stun,
        Dead
    }
    private State currentState = State.Chase;

    private Transform target;
    private NavMeshAgent agent;
    private HealthComponent hc;

    private string[] attackAnimTagOptions = { "Left", "Right", "Both" };
    private string attackAnimTag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        target = ServiceLocator.instance.GetService<PlayerMovement>().transform;

        hc = GetComponent<HealthComponent>();
        hc.onDeath += () =>
        {
            SwitchState(State.Dead);
        };

        SwitchState(State.Chase);
    }

    private void FixedUpdate()
    {
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
            chargeFX.Stop();
        }

        // ENTER
        if (nextState == State.Chase)
        {
            agent.isStopped = false;

            spreadTargetOffset = Random.onUnitSphere * spreadRange;
            spreadTargetOffset.y = 0f;
        }
        else if (nextState == State.Attack)
        {
            agent.isStopped = true;

            attackAnimTag = attackAnimTagOptions[Random.Range(0, attackAnimTagOptions.Length)];
            anim.SetBool($"{attackAnimTag} Aim", true);

            if (attackRoutineRef != null)
                StopCoroutine(attackRoutineRef);

            attackRoutineRef = StartCoroutine(AttackRoutine());
        }
        else if (nextState == State.Stun)
        {
            agent.isStopped = true;
            anim.SetBool("Defend", true);
        }
        else if (nextState == State.Dead)
        {
            agent.isStopped = true;
            anim.SetTrigger("Die");
        }

        currentState = nextState;
    }

    private void ChaseTick()
    {
        if (NavMesh.SamplePosition(target.position + spreadTargetOffset, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        if (Vector3.Distance(transform.position, target.position) <= attackRange)
            if (!Physics.Raycast(transform.position, target.position, attackRange, whatIsGround))
                SwitchState(State.Attack);
    }

    private void AttackTick()
    {
        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);

        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            StopCoroutine(attackRoutineRef);
            anim.SetBool($"{attackAnimTag} Aim", false);

            SwitchState(State.Chase);
        }
    }

    private void StunTick()
    {

    }

    private void DeadTick()
    {

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
            proj.Fire(attackDmg, projectileSpeed, target);
        }
    }
}
