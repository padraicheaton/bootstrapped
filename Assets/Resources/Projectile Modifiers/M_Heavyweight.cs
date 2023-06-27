using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Heavyweight : ProjectileModifier
{
    private float massMultiplier = 2f;
    private float damageMultiplier = 1.25f;

    private float continuousDownwardForce = 2f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.mass *= massMultiplier;
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce(Vector3.down * continuousDownwardForce, ForceMode.Acceleration);
    }
}
