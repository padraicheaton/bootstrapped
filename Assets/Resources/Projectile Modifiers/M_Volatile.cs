using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volatile : ProjectileModifier
{
    private float damageIncreaseMultiplier = 1.1f;
    private float damageIncreaseInterval = 0.5f;

    private float timer = 0f;

    private bool expand = false;

    public override void OnModifierApplied()
    {
    }

    public override void TickModifier(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= damageIncreaseInterval)
        {
            expand = !expand;

            if (expand)
            {
                projectileComponent.MultiplyDamageAmount(damageIncreaseMultiplier);
                projectileComponent.MultiplyProjectileScale(2f);
            }
            else
            {
                projectileComponent.MultiplyDamageAmount(1 / damageIncreaseMultiplier);
                projectileComponent.MultiplyProjectileScale(1 / 2f);
            }

            timer = 0f;
        }
    }
}
