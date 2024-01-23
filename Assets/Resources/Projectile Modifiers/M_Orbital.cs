using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Orbital : ProjectileModifier
{
    private float moveForce = 8f;
    private Transform target;

    public override void OnModifierApplied()
    {
        projectileComponent.MultiplyTimeToLive(2f);

        SearchForTarget();

        if (target)
            projectileRigidbody.velocity = Vector3.Lerp(projectileRigidbody.velocity, Vector3.zero, 0.75f);
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

            Vector3 dir = projectileTransform.forward + projectileTransform.right * 1.5f;

            projectileRigidbody.AddForce(dir * moveForce, ForceMode.Force);

            projectileRigidbody.velocity = Vector3.Lerp(projectileRigidbody.velocity, dir * moveForce, Time.deltaTime * moveForce);
        }
    }

    private void SearchForTarget()
    {
        target = ServiceLocator.instance.GetService<PlayerCamera>().transform;
    }
}
