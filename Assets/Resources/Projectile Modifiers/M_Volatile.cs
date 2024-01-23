using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Volatile : ProjectileModifier
{
    private float damageIncreaseMultiplier = 2f;
    private float damageIncreaseInterval, maxDamageIncreaseInterval = 0.5f;

    private float timer = 0f;

    private bool expand = false;

    public override void OnModifierApplied()
    {
        UpdateInterval();
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
                projectileComponent.MultiplyProjectileScale(damageIncreaseMultiplier);
            }
            else
            {
                projectileComponent.MultiplyDamageAmount(1 / damageIncreaseMultiplier);
                projectileComponent.MultiplyProjectileScale(1 / damageIncreaseMultiplier);
            }

            timer = 0f;

            UpdateInterval();
        }
    }

    private void UpdateInterval()
    {
        damageIncreaseInterval = Random.Range(0f, maxDamageIncreaseInterval);
    }
}
