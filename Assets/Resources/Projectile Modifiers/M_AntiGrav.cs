using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AntiGrav : ProjectileModifier
{
    public override void OnModifierApplied()
    {
        projectileRigidbody.useGravity = false;
        projectileRigidbody.velocity = projectileRigidbody.velocity.normalized * projectileRigidbody.velocity.magnitude / 2f;
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
