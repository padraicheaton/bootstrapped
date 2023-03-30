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
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float damage;
    [SerializeField] protected EffectData effect;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float weaponDropChance;

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

            MovementBehaviour();

            if (target && Vector3.Distance(transform.position, target.position) <= attackRange && timeSinceLastAttacked > attackDelay)
            {
                ApplyEffectToTarget();
            }
        }

        timeSinceLastAttacked += Time.deltaTime;
    }

    protected virtual void MovementBehaviour()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, detectionRange, groundLayer))
        {
            Jump();
        }

        Vector3 movementDirection = target.position - transform.position;
        movementDirection.y = 0f;

        if (movementDirection.magnitude > attackRange)
        {
            movementDirection.Normalize();

            rb.MovePosition(transform.position + movementDirection * (moveSpeed * movementMultiplier) * Time.deltaTime);

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

            Debug.Log(appliedEffect.GetType().ToString());

            if (appliedEffect is E_Knockback)
            {
                ((E_Knockback)appliedEffect).SetMultiplier(50f);
                Debug.Log("Enemy Knocking Player");
            }

            appliedEffect.OnEffectApplied(hc, damage * damageMultiplier, gameObject);
        }

        timeSinceLastAttacked = 0f;
    }

    private void Jump()
    {
        if (IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
