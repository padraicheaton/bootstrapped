using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ElectroStun : BaseEffect
{
    private float damageToDurationScalar = 0.25f;
    private RigidbodyAgent agentController;

    private float duration;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        duration = damage * damageToDurationScalar;

        StartCoroutine(DelayedSetup());
    }

    private IEnumerator DelayedSetup()
    {
        // This is required as some effects may be applied at the same frame
        yield return new WaitForEndOfFrame();

        // Multiply duration if already applied effect
        List<E_ElectroStun> existingEffects = GetEffectObjectsOnParent<E_ElectroStun>();
        existingEffects.Remove(this);

        if (existingEffects.Count > 0)
        {
            existingEffects[0].AddDuration(duration);

            Destroy(gameObject);

            yield break;
        }
        else
        {
            agentController = affectedCharacterHealth.GetComponent<RigidbodyAgent>();

            agentController.SetCanMove(false);

            StartCoroutine(RevertAfterDelay());
        }
    }

    public void AddDuration(float amt)
    {
        duration += amt;
    }

    private IEnumerator RevertAfterDelay()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        agentController.SetCanMove(true);

        Destroy(gameObject);
    }
}
