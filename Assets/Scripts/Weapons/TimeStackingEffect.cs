using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeStackingEffect : BaseEffect
{
    // This class is to be used for those effects that simply add duration when applied again to the target
    private float duration;

    public override void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        base.OnEffectApplied(hc, damage, projectile);

        // Add stacking logic here
        List<TimeStackingEffect> allStackingEffects = GetEffectObjectsOnParent<TimeStackingEffect>();

        // Remove this one
        allStackingEffects.Remove(this);

        // Remove all that are of different types
        for (int i = allStackingEffects.Count - 1; i >= 0; i--)
        {
            if (allStackingEffects[i].GetType() != this.GetType())
                allStackingEffects.RemoveAt(i);
        }

        // This now leaves a list only of relevant effects currently applied to the enemy
        if (allStackingEffects.Count > 0)
        {
            // If there is another version of this effect already active on the enemy, add to its duration
            allStackingEffects[0].AddDuration(GetStackDuration());

            Destroy(gameObject);
        }
        else
        {
            // If not, this is the first instance of this effect, kick it off
            AddDuration(GetStackDuration());

            StartCoroutine(RevertAfterDelay());
        }
    }

    public void AddDuration(float amount)
    {
        duration += amount;
    }

    protected override float GetApplicationDamageMultiplier()
    {
        return 0.25f;
    }

    protected abstract float GetStackDuration();

    private IEnumerator RevertAfterDelay()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        OnEffectExpired();

        Destroy(gameObject);
    }

    protected virtual void OnEffectExpired()
    {

    }
}
