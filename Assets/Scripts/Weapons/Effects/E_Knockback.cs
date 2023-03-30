using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Knockback : BaseEffect
{
    protected float damageToForceScalar = 0.5f;
    protected float forceMultiplier = 1f;

    private Vector3 accelDir = Vector3.zero;
    private Rigidbody rb;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        if (affectedCharacterHealth.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float knockBackForce = damage * damageToForceScalar * forceMultiplier;

            Vector3 pointOfReference = ServiceLocator.instance.GetService<PlayerCamera>().transform.position;

            Vector3 dirFromProjectileToTarget = (affectedCharacterHealth.transform.position - pointOfReference).normalized;

            accelDir = dirFromProjectileToTarget * knockBackForce;

            this.rb = rb;

            // Cancel out existing forces
            rb.velocity = dirFromProjectileToTarget;

            rb.AddForce(accelDir, ForceMode.Impulse);
        }
    }

    protected override float GetApplicationDamageMultiplier()
    {
        return 0.5f;
    }
}
