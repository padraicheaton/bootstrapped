using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Explosive : ProjectileModifier
{
    private float explosionRadiusMultiplier = 1.5f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyExplosionRadius(explosionRadiusMultiplier);

        projectileComponent.AddLayerToAffectingLayers(LayerMask.NameToLayer("Ground"));
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
