using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Rebound : ProjectileModifier
{
    private int livesToAdd = 1;
    private float noClipWindow = 0.1f;

    private float searchRadius = 8f;

    private Vector3 storedVelocity;

    public override void OnModifierApplied()
    {
        projectileComponent.AddLives(livesToAdd);

        projectileComponent.onCollided += other =>
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                projectileComponent.DisableColliderForDuration(noClipWindow);

                projectileRigidbody.velocity = GetNearestEnemyDir();
            }
        };
    }

    public override void TickModifier(float deltaTime)
    {
        storedVelocity = projectileRigidbody.velocity;
    }

    private Vector3 GetNearestEnemyDir()
    {
        foreach (Collider coll in Physics.OverlapSphere(projectileTransform.position, searchRadius, projectileComponent.GetAffectingLayers()))
        {
            // Ensure bounce to different enemy than current one
            if (Vector3.Distance(coll.transform.position, projectileTransform.position) > 1f)
                return (coll.transform.position - projectileTransform.position).normalized * storedVelocity.magnitude;
        }

        return storedVelocity;
    }
}
