using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Orbital : ProjectileModifier
{
    private float searchRadius = 15f;
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
            try
            {
                projectileTransform.LookAt(target);
            }
            catch
            {
                SearchForTarget();
            }

            Vector3 dir = projectileTransform.forward + projectileTransform.right * 0.25f;

            projectileRigidbody.AddForce(dir * moveForce, ForceMode.Acceleration);
        }
    }

    private void SearchForTarget()
    {
        target = ServiceLocator.instance.GetService<PlayerCamera>().transform;
    }
}
