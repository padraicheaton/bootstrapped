using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgent : RigidbodyAgent
{
    [Header("Flying Settings")]
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverAccel;
    [SerializeField] private float pseudoFriction;
    [SerializeField] private LineRenderer laserLine;

    protected override void MovementBehaviour()
    {
        Vector3 movementDirection = target.position - transform.position;

        if (movementDirection.magnitude > attackRange)
        {
            movementDirection.Normalize();

            rb.MovePosition(transform.position + movementDirection * (moveSpeed * movementMultiplier) * Time.deltaTime);
        }

        if (ShouldMoveUpwards())
        {
            rb.AddForce(Vector3.up * hoverAccel * 2f, ForceMode.Acceleration);
        }
        else
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(rb.velocity.x, 0f, rb.velocity.z), Time.deltaTime * 2f);

        // Apply psuedo friction
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * pseudoFriction);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime);

        UpdateLaser((target.position - transform.position).magnitude < attackRange);
    }

    private bool ShouldMoveUpwards()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, hoverHeight, groundLayer) || target.position.y > transform.position.y;
    }

    private void UpdateLaser(bool isAttacking)
    {
        laserLine.enabled = isAttacking;

        if (isAttacking && target)
        {
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, target.position);
        }
    }
}
