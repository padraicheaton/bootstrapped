using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volatile : ProjectileModifier
{
    private float timeMultiplier = 0.25f;
    private float damageMultiplier = 1.25f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyTimeToLive(timeMultiplier);
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
