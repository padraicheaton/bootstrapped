using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Homing : ProjectileModifier
{
    private float searchRadius = 20f;
    private float moveForce = 12f;
    private Transform target;

    public override void OnModifierApplied()
    {
        SearchForTarget();

        if (target)
            projectileRigidbody.velocity = Vector3.Lerp(projectileRigidbody.velocity, Vector3.Normalize(target.position - projectileTransform.position) * moveForce, 0.75f);
    }

    public override void TickModifier(float deltaTime)
    {
        if (target == null)
            SearchForTarget();

        if (target)
        {
            projectileRigidbody.AddForce((target.position - projectileTransform.position).normalized * moveForce, ForceMode.Force);

            // Slowly override other forces in favour of this one
            projectileRigidbody.velocity = Vector3.Lerp(projectileRigidbody.velocity, (target.position - projectileTransform.position).normalized, Time.deltaTime);
        }
    }

    private void SearchForTarget()
    {
        // foreach (Collider coll in Physics.OverlapSphere(projectileTransform.position, searchRadius, projectileComponent.GetAffectingLayers()))
        // {
        //     target = coll.transform;
        //     break;
        // }

        Collider[] foundEnemies = Physics.OverlapSphere(projectileTransform.position, searchRadius, projectileComponent.GetAffectingLayers());

        if (foundEnemies.Length > 0)
        {
            target = foundEnemies[Random.Range(0, foundEnemies.Length)].transform;
        }
    }
}
