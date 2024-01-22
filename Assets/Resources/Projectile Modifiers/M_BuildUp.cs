using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BuildUp : ProjectileModifier
{
    private float damageIncreaseMultiplier = 1.1f;
    private float damageIncreaseInterval = 0.5f;
    private float damageMultiplier = 0.5f;

    private float timer = 0f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= damageIncreaseInterval)
        {
            projectileComponent.MultiplyDamageAmount(damageIncreaseMultiplier);
            projectileComponent.MultiplyProjectileScale(1.25f);

            timer = 0f;
        }
    }
}
