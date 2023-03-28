using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Condense : ProjectileModifier
{
    private float scaleModifier = 0.5f;
    private float damageMultiplier = 1.25f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyProjectileScale(scaleModifier);
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
