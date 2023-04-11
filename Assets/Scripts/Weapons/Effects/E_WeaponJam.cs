using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_WeaponJam : BaseEffect
{
    private float agentDamageMultiplier = 0.5f;
    private float damageToDurationScalar = 0.25f;
    private RigidbodyAgent agentController;

    private float duration;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        duration = damage * damageToDurationScalar;

        StartCoroutine(DelayedSetup());
    }

    protected override float GetApplicationDamageMultiplier()
    {
        return 0.25f;
    }

    private IEnumerator DelayedSetup()
    {
        // This is required as some effects may be applied at the same frame
        yield return new WaitForEndOfFrame();

        // Multiply duration if already applied effect
        List<E_WeaponJam> existingEffects = GetEffectObjectsOnParent<E_WeaponJam>();
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

            if (agentController)
                agentController.AlterDamage(agentDamageMultiplier);

            StartCoroutine(RevertAfterDelay());
        }
    }

    public void AddDuration(float amt)
    {
        duration += amt;

        Debug.Log($"Added duration {amt}, total duration {duration}");
    }

    private IEnumerator RevertAfterDelay()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        if (agentController)
            agentController.ResetDamageMultiplier();

        Destroy(gameObject);
    }
}
