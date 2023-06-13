using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_IceSlow : TimeStackingEffect
{
    private float totalDuration = 10f;
    private NavmeshRobot agentController;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        agentController = affectedCharacterHealth.GetComponent<NavmeshRobot>();

        agentController.SetSlowedState(true);
    }

    protected override void OnEffectExpired()
    {
        agentController.SetSlowedState(false);
    }

    protected override float GetStackDuration()
    {
        return totalDuration;
    }
}
