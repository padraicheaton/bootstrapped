using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Preserved : ProjectileModifier
{
    private float timeMultiplier = 8f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyTimeToLive(timeMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
