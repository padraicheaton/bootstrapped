using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    /*
    Effects are applied by weapons, and are instantiated as children of those applied to
    */
    public Sprite icon;

    protected HealthComponent affectedCharacterHealth;

    public virtual void OnEffectApplied(HealthComponent hc, float damage, GameObject projectile)
    {
        affectedCharacterHealth = hc;
    }

    protected List<GameObject> GetEffectObjectsOnParent<T>()
    {
        List<GameObject> effects = new List<GameObject>();

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform child = transform.parent.GetChild(i);

            if (child.GetComponent<T>() != null)
                effects.Add(child.gameObject);
        }

        return effects;
    }
}