using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Lob : ProjectileModifier
{
    private float launchForce = 15f;
    private float continuousForwardAccel = 10f;

    private float timer, remainingForceTime = 1f;

    public override void OnModifierApplied()
    {
        projectileRigidbody.velocity = new Vector3(projectileRigidbody.velocity.x / 2f, 0f, projectileRigidbody.velocity.z / 2f);
        projectileRigidbody.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }

    public override void TickModifier(float deltaTime)
    {
        timer += deltaTime;

        if (timer < remainingForceTime)
            projectileRigidbody.AddForce(Vector3.up * continuousForwardAccel, ForceMode.Force);
        else if (timer < remainingForceTime * 2f)
            projectileRigidbody.AddForce(Vector3.down * continuousForwardAccel * 2f, ForceMode.Force);
    }
}
