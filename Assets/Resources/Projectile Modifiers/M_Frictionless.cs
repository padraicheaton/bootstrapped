using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Frictionless : ProjectileModifier
{
    private float bonusForceMultiplier = 0.25f;
    private Collider projectileCollider;

    public override void OnModifierApplied()
    {
        projectileCollider = projectileTransform.GetComponent<Collider>();

        PhysicMaterial slipperyMaterial = new PhysicMaterial();

        if (projectileCollider.material != null)
            slipperyMaterial = projectileCollider.material;

        slipperyMaterial.dynamicFriction = 0f;
        slipperyMaterial.staticFriction = 0f;

        projectileCollider.material = slipperyMaterial;
    }

    public override void TickModifier(float deltaTime)
    {
        projectileRigidbody.AddForce(projectileRigidbody.velocity.normalized * bonusForceMultiplier, ForceMode.Acceleration);
    }
}
