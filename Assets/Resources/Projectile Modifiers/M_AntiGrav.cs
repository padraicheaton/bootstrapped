using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AntiGrav : ProjectileModifier
{
    public override void OnModifierApplied()
    {
        projectileRigidbody.useGravity = false;
        projectileRigidbody.velocity = new Vector3(projectileRigidbody.velocity.x, 0f, projectileRigidbody.velocity.z);
    }

    public override void TickModifier(float deltaTime)
    {

    }
}
