using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ElectroStun : TimeStackingEffect
{
    private float totalDuration = 3f;
    private NavmeshRobot agentController;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        agentController = affectedCharacterHealth.GetComponent<NavmeshRobot>();

        agentController.SwitchState(NavmeshRobot.State.Stun);
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
