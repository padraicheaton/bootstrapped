using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Knockback : BaseEffect
{
    protected float damageToForceScalar = 10f;
    protected float forceMultiplier = 1f;
    protected float externalMultiplier = 1f;

    private Vector3 accelDir = Vector3.zero;
    private Rigidbody rb;

    public void SetMultiplier(float multiplier)
    {
        externalMultiplier = multiplier;
    }

    private Vector3 GetKnockbackForceVector(float damage, GameObject projectile)
    {
        float knockBackForce = damage * damageToForceScalar * forceMultiplier;

        Vector3 pointOfReference = projectile.transform.position;

        Vector3 dirFromProjectileToTarget = (affectedCharacterHealth.transform.position - pointOfReference).normalized;

        return dirFromProjectileToTarget * knockBackForce * externalMultiplier;
    }

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        if (affectedCharacterHealth.TryGetComponent<NavmeshRobot>(out NavmeshRobot robot))
        {
            robot.KnockAgent(GetKnockbackForceVector(damage, projectile));
        }
        else if (affectedCharacterHealth.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            accelDir = GetKnockbackForceVector(damage, projectile);

            this.rb = rb;

            // Cancel out existing forces
            rb.velocity = accelDir.normalized;

            rb.AddForce(accelDir, ForceMode.Impulse);
        }
    }

    protected override float GetApplicationDamageMultiplier()
    {
        return 0.2f;
    }
}
