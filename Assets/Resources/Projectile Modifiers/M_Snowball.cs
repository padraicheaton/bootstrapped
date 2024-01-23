using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Snowball : ProjectileModifier
{
    private float damageIncreaseMultiplier = 1.2f;
    private float damageIncreaseInterval = 0.25f;
    private float damageMultiplier = 0.5f;

    private int timesIncreased, maxIncreaseTimes = 10;

    private float timer = 0f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyDamageAmount(damageMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= damageIncreaseInterval && timesIncreased < maxIncreaseTimes)
        {
            projectileComponent.MultiplyDamageAmount(damageIncreaseMultiplier);
            projectileComponent.MultiplyProjectileScale(damageIncreaseMultiplier);

            timer = 0f;

            timesIncreased++;
        }
    }
}
