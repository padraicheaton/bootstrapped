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

            if (projectileTransform.parent && other.transform)
                projectileTransform.parent.SetParent(other.transform);
        };
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
