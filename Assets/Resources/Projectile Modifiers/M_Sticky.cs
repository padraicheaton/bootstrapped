using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Sticky : ProjectileModifier
{
    public override void OnModifierApplied()
    {
        projectileComponent.onCollided += other =>
        {
            projectileRigidbody.velocity = Vector3.zero;
            projectileRigidbody.isKinematic = true;

            projectileComponent.MultiplyExplosionRadius(1.2f);
            projectileComponent.MultiplyTimeToLive(1.5f);
        };
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
