using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Launch : ProjectileModifier
{
    private float launchForce = 30f;
    private float continuousForwardAccel = 1f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.velocity = new Vector3(projectileRigidbody.velocity.x, 0f, projectileRigidbody.velocity.z);
        projectileRigidbody.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce(Vector3.up * continuousForwardAccel, ForceMode.Acceleration);
    }
}
