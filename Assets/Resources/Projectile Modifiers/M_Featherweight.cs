using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Featherweight : ProjectileModifier
{
    private float massMultiplier = 0.5f;
    private float gravityCounterForce = 8f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.mass *= massMultiplier;
    }

    public override void TickModifier(float deltaTime)
    {
        if (projectileRigidbody.velocity.y < 0)
            projectileRigidbody.AddForce(Vector3.up * gravityCounterForce, ForceMode.Acceleration);
    }
}
