using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Spiral : ProjectileModifier
{
    //* Settings
    private float spiralRadius = 1f;
    private float spiralSpeed = 2f;

    private Transform childTransform;
    private Vector3 spiralPosition = Vector3.zero;

    public override void OnModifierApplied()
    {
    }

    public override void TickModifier(float deltaTime)
    {
        spiralPosition.x = Mathf.Sin(Time.time * spiralSpeed);

        spiralPosition *= spiralRadius;

        projectileTransform.localPosition = Vector3.Lerp(projectileTransform.localPosition, spiralPosition, deltaTime * spiralSpeed * 10f);
    }
}
