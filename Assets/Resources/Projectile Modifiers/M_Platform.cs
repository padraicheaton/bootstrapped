using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Platform : ProjectileModifier
{
    private float horizontalModifier = 2f;
    private float verticalModifier = 0.75f;

    private float platformLifetime = 10f;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyProjectileScale(new Vector3(horizontalModifier, verticalModifier, horizontalModifier));
        //projectileComponent.AddLayerToAffectingLayers(LayerMask.NameToLayer("Ground"));

        projectileComponent.onDestroyed += OnDestroyed;
    }

    public override void TickModifier(float deltaTime)
    {

    }

    private void OnDestroyed()
    {
        ServiceLocator.instance.GetService<ProjectileHelper>().OnPlatformProjectileDestroyed(projectileTransform.gameObject, platformLifetime);
    }
}
