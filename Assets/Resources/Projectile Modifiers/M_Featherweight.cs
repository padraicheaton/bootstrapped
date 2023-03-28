using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Featherweight : ProjectileModifier
{
    private float massMultiplier = 0.5f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.mass *= massMultiplier;
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
