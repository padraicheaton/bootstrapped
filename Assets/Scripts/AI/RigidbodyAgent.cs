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
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private float targetSearchRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private float damage;
    [SerializeField] private EffectData effect;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;

    private Rigidbody rb;
    private Transform target;
    private Allegiance currentAllegiance = Allegiance.Enemy;
    private float timeSinceLastAttacked;
    private bool CanMove = true;

    private float damageMultiplier = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateTarget();
    }

    private void Update()
    {
        if (CanMove)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, detectionRange, groundLayer))
            {
                Jump();
            }

            if (target == null)
                UpdateTarget();

            Vector3 movementDirection = target.position - transform.position;
            movementDirection.y = 0f;

            if (movementDirection.magnitude > attackRange)
            {
                movementDirection.Normalize();

                rb.MovePosition(transform.position + movementDirection * moveSpeed * Time.deltaTime);

                transform.rotation = Quaternion.LookRotation(movementDirection);
            }
            else if (timeSinceLastAttacked > attackDelay)
            {
                // Close enough to target to apply effect
                ApplyEffectToTarget();
            }
        }

        timeSinceLastAttacked += Time.deltaTime;
    }

    private void ApplyEffectToTarget()
    {
        if (target.TryGetComponent<HealthComponent>(out HealthComponent hc))
        {
            BaseEffect appliedEffect = Instantiate(effect.prefab, target).GetComponent<BaseEffect>();
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

    private void UpdateTarget()
    {
        if (currentAllegiance == Allegiance.Enemy)
            target = ServiceLocator.instance.GetService<PlayerMovement>().transform;
        else if (currentAllegiance == Allegiance.Player)
        {
            float minDist = float.MaxValue;

            foreach (Collider coll in Physics.OverlapSphere(transform.position, targetSearchRadius, enemyLayer))
            {
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
}
