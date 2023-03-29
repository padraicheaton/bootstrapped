using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Damage : BaseEffect
{
    [SerializeField] private float vfxDuration = 1f;
    private float damageMultiplier = 1.5f;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        affectedCharacterHealth.TakeDamage(damage * damageMultiplier);

        Destroy(gameObject, vfxDuration);
    }
}
