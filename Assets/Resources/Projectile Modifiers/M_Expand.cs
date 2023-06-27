using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Expand : ProjectileModifier
{
    private float scaleModifier = 4f;
    private float damageMultiplier = 0.7f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyProjectileScale(scaleModifier);
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
