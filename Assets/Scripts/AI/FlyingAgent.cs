using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAgent : RigidbodyAgent
{
    [Header("Flying Settings")]
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverAccel;

    protected override void MovementBehaviour()
    {
        Vector3 movementDirection = target.position - transform.position;

        if (movementDirection.magnitude > attackRange)
        {
            movementDirection.Normalize();

            rb.MovePosition(transform.position + movementDirection * (moveSpeed * movementMultiplier) * Time.deltaTime);
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, hoverHeight, groundLayer))
        {
            rb.AddForce(Vector3.up * hoverAccel, ForceMode.Acceleration);
        }
        else
            rb.AddForce(Vector3.down * hoverAccel, ForceMode.Acceleration);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime);
    }
}
