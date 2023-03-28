using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Curve : ProjectileModifier
{
    private Vector3[] possibleCurveDirs = new Vector3[] { Vector3.left, Vector3.right };

    private float curveForce = 20f;
    private float secondaryCurveAcceleration = 3f;
    private Vector3 curveDir;

    public override void OnModifierApplied()
    {
        curveDir = possibleCurveDirs[Random.Range(0, possibleCurveDirs.Length)];

        projectileRigidbody.AddForce(curveDir * -curveForce, ForceMode.Impulse);
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce((curveDir + projectileTransform.forward).normalized * secondaryCurveAcceleration, ForceMode.Acceleration);
    }
}
