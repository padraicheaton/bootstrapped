using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Hacking : TimeStackingEffect
{
    private float totalDuration = 5f;
    private NavmeshRobot agentController;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        agentController = affectedCharacterHealth.GetComponent<NavmeshRobot>();

        agentController.SwitchState(NavmeshRobot.State.Hacked);
    }

    protected override void OnEffectExpired()
    {
        agentController.SwitchState(NavmeshRobot.State.Chase);
    }

    protected override float GetStackDuration()
    {
        return totalDuration;
    }
}
