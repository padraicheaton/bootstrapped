using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Homing : ProjectileModifier
{
    private float searchRadius = 15f;
    private float moveForce = 4f;
    private Transform target;

    public override void OnModifierApplied()
    {
        SearchForTarget();
    }

    public override void TickModifier(float deltaTime)
    {
        if (target == null)
            SearchForTarget();

        if (target)
        {
            try
            {
                projectileTransform.LookAt(target);
            }
            catch
            {
                SearchForTarget();
            }

            projectileRigidbody.AddForce(projectileTransform.forward * moveForce, ForceMode.Acceleration);
        }
    }

    private void SearchForTarget()
    {
        foreach (Collider coll in Physics.OverlapSphere(projectileTransform.position, searchRadius, projectileComponent.GetAffectingLayers()))
        {
            target = coll.transform;
            break;
        }
    }
}
