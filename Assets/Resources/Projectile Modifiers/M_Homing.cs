using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Homing : ProjectileModifier
{
    private float searchRadius = 20f;
    private float moveForce = 8f;
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
            projectileRigidbody.AddForce((target.position - projectileTransform.position).normalized * moveForce, ForceMode.Acceleration);

            // Slowly override other forces in favour of this one
            projectileRigidbody.velocity = Vector3.Lerp(projectileRigidbody.velocity, (target.position - projectileTransform.position).normalized, Time.deltaTime);
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
