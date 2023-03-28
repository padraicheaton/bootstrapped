using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Launch : ProjectileModifier
{
    private float launchForce = 40f;
    private float continuousForwardAccel = 1f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.velocity = new Vector3(projectileRigidbody.velocity.x, 0f, projectileRigidbody.velocity.z);
        projectileRigidbody.AddForce(projectileTransform.up * launchForce, ForceMode.Impulse);
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce(projectileTransform.forward.normalized * continuousForwardAccel, ForceMode.Acceleration);
    }
}
