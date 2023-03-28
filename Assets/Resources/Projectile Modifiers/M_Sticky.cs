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
        };
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
