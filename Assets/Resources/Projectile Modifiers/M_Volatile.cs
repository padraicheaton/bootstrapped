using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volatile : ProjectileModifier
{
    private float damageDecayMultiplier = 0.9f;
    private float damageDecayInterval = 0.5f;
    private float damageMultiplier = 2f;

    private float timer = 0f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= damageDecayInterval)
        {
            projectileComponent.MultiplyDamageAmount(damageDecayMultiplier);
        }
    }
}
