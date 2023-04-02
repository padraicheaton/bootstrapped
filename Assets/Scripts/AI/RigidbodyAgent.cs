using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyAgent : MonoBehaviour
{
    public enum Allegiance
    {
        Player,
        Enemy
    }

    [Header("Settings")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float targetSearchRadius;
    [SerializeField] protected float damage;
    [SerializeField] protected EffectData effect;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float weaponDropChance;
    [SerializeField] private float springStrenth = 2f;
    [SerializeField] private float springDamper = 2f;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private float preferredHoverHeight = 2f;
    [SerializeField] private float gravityStrength = 2f;
    [SerializeField] private float psuedoFriction = 5f;
    [SerializeField] private float aviodanceRange = 2f;
    [SerializeField] private float knockMultiplier = 50f;

    protected Rigidbody rb;
    protected Transform target;
    protected Allegiance currentAllegiance = Allegiance.Enemy;
    protected float timeSinceLastAttacked;
    protected bool CanMove = true;

    protected float damageMultiplier = 1f;
    protected float movementMultiplier = 1f;

    private HealthComponent hc;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateTarget();

        hc = GetComponent<HealthComponent>();

        hc.onDeath += OnDeath;
    }

    private void Update()
    {
        if (CanMove)
        {
            if (target == null)
                UpdateTarget();

            ApplyHoverForce();

            MovementBehaviour();

            ApplyPseudoFriction();

            AvoidNeighbours();

            if (target && Vector3.Distance(transform.position, target.position) <= attackRange && timeSinceLastAttacked > attackDelay)
            {
                ApplyEffectToTarget();
            }
        }

        timeSinceLastAttacked += Time.deltaTime;
    }

    protected virtual void MovementBehaviour()
    {
        Vector3 movementDirection = target.position - transform.position;
        movementDirection.y = 0f;
        Vector3 obstacleAvoidanceForce = Vector3.zero;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, detectionRange, groundLayer))
        {
            // Counteract the spring force pulling the agent down
            obstacleAvoidanceForce = Vector3.up * springStrenth * 2f;
        }

        if (movementDirection.magnitude >= attackRange)
        {
            movementDirection.Normalize();

            if (obstacleAvoidanceForce.y > 0)
                movementDirection.y = obstacleAvoidanceForce.y;

            //rb.MovePosition(transform.position + movementDirection * (moveSpeed * movementMultiplier) * Time.deltaTime);
            if (rb.velocity.magnitude < moveSpeed)
                rb.AddForce(movementDirection * (moveSpeed * movementMultiplier) + obstacleAvoidanceForce);

            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }

    private void ApplyEffectToTarget()
    {
        // Make sure the target is not obstructed by terrain
        if (Physics.Linecast(transform.position, target.position, groundLayer))
        {
            return;
        }

        if (target.TryGetComponent<HealthComponent>(out HealthComponent hc))
        {
            BaseEffect appliedEffect = Instantiate(effect.prefab, target).GetComponent<BaseEffect>();

            if (appliedEffect is E_Knockback || appliedEffect.GetType().IsSubclassOf(typeof(E_Knockback)))
            {
                Debug.Log(appliedEffect.ToString());
                ((E_Knockback)appliedEffect).SetMultiplier(knockMultiplier);
            }

            appliedEffect.OnEffectApplied(hc, damage * damageMultiplier, gameObject);
        }

        timeSinceLastAttacked = 0f;
    }

    private void ApplyHoverForce()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
        {
            // Trying to copy code from very very valet
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down);

            float rayDirVel = Vector3.Dot(rayDir, vel);

            float x = hit.distance - preferredHoverHeight;

            float springForce = (x * springStrenth) - (rayDirVel * springDamper);

            rb.AddForce(rayDir * springForce);
        }
        else
        {
            // Apply a downward force
            rb.AddForce(Vector3.down * gravityStrength);
        }
    }

    private void ApplyPseudoFriction()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * psuedoFriction);
    }

    private void AvoidNeighbours()
    {
        foreach (Collider coll in Physics.OverlapSphere(transform.position, aviodanceRange, enemyLayer))
        {
            Vector3 dirVector = transform.position - coll.transform.position;

            float force = aviodanceRange - dirVector.magnitude;

            rb.AddForce(dirVector * force * 0.25f, ForceMode.Acceleration);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    private void OnDeath()
    {
        if (Random.value < weaponDropChance)
            ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(transform.position + Vector3.up * 0.5f);

        Destroy(gameObject);
    }

    private void UpdateTarget()
    {
        if (currentAllegiance == Allegiance.Enemy)
            target = ServiceLocator.instance.GetService<PlayerMovement>().transform;
        else if (currentAllegiance == Allegiance.Player)
        {
            float minDist = float.MaxValue;

            foreach (Collider coll in Physics.OverlapSphere(transform.position, targetSearchRadius, enemyLayer))
            {
                if (coll.transform == transform) continue;

                if (Vector3.Distance(transform.position, coll.transform.position) < minDist)
                {
                    minDist = Vector3.Distance(transform.position, coll.transform.position);
                    target = coll.transform;
                }
            }
        }
    }

    public void SetAllegiance(Allegiance newAllegiance)
    {
        currentAllegiance = newAllegiance;

        UpdateTarget();
    }

    public void AlterDamage(float multiplier)
    {
        damageMultiplier *= multiplier;
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1f;
    }

    public void SetCanMove(bool CanMove)
    {
        this.CanMove = CanMove;
    }

    public void MultiplyMovementSpeed(float modifier)
    {
        movementMultiplier *= modifier;
    }

    public void DivideMovementSpeed(float modifier)
    {
        movementMultiplier /= modifier;
    }
}
