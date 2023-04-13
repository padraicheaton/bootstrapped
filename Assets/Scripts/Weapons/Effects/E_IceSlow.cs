using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_IceSlow : BaseEffect
{
    private float damageToDurationScalar = 0.5f;
    private float movementSpeedMultiplier = 0.75f;
    private Rigidbody rb;

    private float duration;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        duration = damage * damageToDurationScalar;

        rb = affectedCharacterHealth.GetComponent<Rigidbody>();

        StartCoroutine(RevertAfterDelay());
    }

    private void Update()
    {
        if (rb)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * movementSpeedMultiplier);
        }
    }

    protected override float GetApplicationDamageMultiplier()
    {
        return 0.25f;
    }

    private IEnumerator RevertAfterDelay()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
