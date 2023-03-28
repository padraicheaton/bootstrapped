using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_VelocityBoost : ProjectileModifier
{
    // Settings
    private float launchForce = 50f;

    public override void OnModifierApplied()
    {
        Vector3 launchDir = projectileTransform.forward + (Vector3.up * 0.1f);

        projectileRigidbody.AddForce(launchDir.normalized * launchForce, ForceMode.Impulse);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
