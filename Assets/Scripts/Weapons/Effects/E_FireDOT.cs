using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_FireDOT : TimeStackingEffect
{
    private float totalDuration = 5f;
    private float tickDelay = 0.5f;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        float tickDamage = damage / totalDuration;

        StartCoroutine(DamageOverTime(tickDamage));
    }

    private IEnumerator DamageOverTime(float tickDamage)
    {
        while (true)
        {
            yield return new WaitForSeconds(tickDelay);

            affectedCharacterHealth.TakeDamage(tickDamage);
        }
    }

    protected override float GetStackDuration()
    {
        return totalDuration;
    }
}
