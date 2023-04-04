using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Platform : ProjectileModifier
{
    private float velocityMultiplier = 0.25f;
    private float horizontalModifier = 4f;
    private float verticalModifier = 0.2f;

    private float platformLifetime = 30f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.velocity *= velocityMultiplier;

        projectileComponent.MultiplyProjectileScale(new Vector3(horizontalModifier, verticalModifier, horizontalModifier));

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
