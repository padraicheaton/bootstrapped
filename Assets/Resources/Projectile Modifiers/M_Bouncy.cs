using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Bouncy : ProjectileModifier
{
    private Collider projectileCollider;

    private bool shouldBounceAfterDelay = false;
    private float timer = 0f;
    private float timeToBounce = 0.1f;
    private float bounceForce = 8f;

    public override void OnModifierApplied()
    {
        projectileCollider = projectileTransform.GetComponent<Collider>();

        PhysicMaterial bouncyMaterial = new PhysicMaterial();

        if (projectileCollider.material != null)
            bouncyMaterial = projectileCollider.material;

        bouncyMaterial.dynamicFriction = 0f;
        bouncyMaterial.staticFriction = 0f;
        bouncyMaterial.bounciness = 1f;

        projectileCollider.material = bouncyMaterial;

        projectileComponent.onCollided += OnCollided;
    }

    private void OnCollided(Collision other)
    {
        shouldBounceAfterDelay = true;
        timer = 0f;
    }

    public override void TickModifier(float deltaTime)
    {
        if (shouldBounceAfterDelay)
        {
            timer += Time.deltaTime;

            if (timer >= timeToBounce)
            {
                projectileRigidbody.AddForce(projectileRigidbody.velocity.normalized * bounceForce, ForceMode.Impulse);

                shouldBounceAfterDelay = false;
            }
        }
    }
}
