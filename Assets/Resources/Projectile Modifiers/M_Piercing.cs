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
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                projectileComponent.ExplodeEffect();

                projectileComponent.DisableColliderForDuration(noClipWindow);

                storedVelocity.y = 0f;

                projectileRigidbody.velocity = storedVelocity;
            }
        };
    }

    public override void TickModifier(float deltaTime)
    {
        storedVelocity = projectileRigidbody.velocity;
    }
}
