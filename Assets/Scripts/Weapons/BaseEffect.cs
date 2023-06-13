using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    /*
    Effects are applied by weapons, and are instantiated as children of those applied to
    */

    protected HealthComponent affectedCharacterHealth;

    public virtual void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        affectedCharacterHealth = hc;
        affectedCharacterHealth.TakeDamage(damage * GetApplicationDamageMultiplier());
    }

    protected abstract float GetApplicationDamageMultiplier();

    protected List<T> GetEffectObjectsOnParent<T>()
    {
        List<T> effects = new List<T>();

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform child = transform.parent.GetChild(i);

            if (child.TryGetComponent<T>(out T effect))
                effects.Add(effect);
        }

        return effects;
    }
}