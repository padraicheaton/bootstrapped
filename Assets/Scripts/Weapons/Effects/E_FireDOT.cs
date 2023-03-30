using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_FireDOT : BaseEffect
{
    private float totalDuration = 5f;
    private float tickDelay = 0.5f;
    private float tickDamage;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        int tickTimes = Mathf.RoundToInt(totalDuration / tickDelay);

        tickDamage = damage / tickTimes;

        StartCoroutine(DamageOverTime(tickTimes));
    }

    private IEnumerator DamageOverTime(int tickTimes)
    {
        for (int i = 0; i < tickTimes; i++)
        {
            yield return new WaitForSeconds(tickDelay);

            affectedCharacterHealth.TakeDamage(tickDamage);
        }

        Destroy(gameObject);
    }
}
