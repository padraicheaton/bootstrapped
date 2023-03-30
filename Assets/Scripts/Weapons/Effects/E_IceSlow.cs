using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_IceSlow : BaseEffect
{
    private float damageToDurationScalar = 0.5f;
    private float movementSpeedMultiplier = 0.75f;
    private RigidbodyAgent agentController;

    private float duration;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        duration = damage * damageToDurationScalar;

        agentController = affectedCharacterHealth.GetComponent<RigidbodyAgent>();

        agentController.MultiplyMovementSpeed(movementSpeedMultiplier);

        StartCoroutine(RevertAfterDelay());
    }

    private IEnumerator RevertAfterDelay()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        agentController.DivideMovementSpeed(movementSpeedMultiplier);

        Destroy(gameObject);
    }
}
