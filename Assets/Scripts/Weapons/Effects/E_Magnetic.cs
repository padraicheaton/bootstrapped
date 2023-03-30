using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Magnetic : E_Knockback
{
    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        forceMultiplier = -1f;

        base.OnEffectApplied(hc, damage, projectile);
    }
}
