using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Explosive : ProjectileModifier
{
    private float explosionRadiusMultiplier = 1.25f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyExplosionRadius(explosionRadiusMultiplier);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
