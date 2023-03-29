using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Piercing : ProjectileModifier
{
    private int livesToAdd = 1;
    private float noClipWindow = 0.25f;

    private Vector3 storedVelocity;

    public override void OnModifierApplied()
    {
        projectileComponent.AddLives(livesToAdd);

        projectileComponent.onCollided += other =>
        {
            projectileComponent.DisableColliderForDuration(noClipWindow);

            projectileRigidbody.velocity = storedVelocity;
        };
    }

    public override void TickModifier(float deltaTime)
    {
        storedVelocity = projectileRigidbody.velocity;
    }
}
