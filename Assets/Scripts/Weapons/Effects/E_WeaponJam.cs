using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_WeaponJam : TimeStackingEffect
{
    private float totalDuration = 7f;
    private NavmeshRobot agentController;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        agentController = affectedCharacterHealth.GetComponent<NavmeshRobot>();

        agentController.SetJammedState(true);
    }

    protected override void OnEffectExpired()
    {
        agentController.SetJammedState(false);
    }

    protected override float GetStackDuration()
    {
        return totalDuration;
    }
}
