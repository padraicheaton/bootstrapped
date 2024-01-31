using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Curve : ProjectileModifier
{
    private float curveAcceleration = 5f;

    private int multiplier = -1;

    public override void OnModifierApplied()
    {
        if (Random.value < 0.5f)
            multiplier = 1;
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce(projectileTransform.right * multiplier * curveAcceleration, ForceMode.Force);
    }
}
