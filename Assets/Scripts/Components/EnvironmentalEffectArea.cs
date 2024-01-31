using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalEffectArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxComponent;

    [Header("Settings")]
    [SerializeField] private EffectData effect;
    [SerializeField] private float damage;
    [SerializeField] private float applicationDelay;
    [SerializeField] private float forceMultiplier;


    private List<GameObject> affectedObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthComponent>(out HealthComponent hc))
        {
            BaseEffect effectScript = Instantiate(effect.prefab, other.transform).GetComponent<BaseEffect>();

            if (effectScript is E_Knockback || effectScript.GetType().IsSubclassOf(typeof(E_Knockback)))
            {
                ((E_Knockback)effectScript).SetMultiplier(forceMultiplier);
            }

            effectScript.OnEffectApplied(hc, damage, gameObject);

            affectedObjects.Add(other.gameObject);

            StartCoroutine(RemoveGameObjectAfterDelay(other.gameObject));

            if (sfxComponent != null)
                sfxComponent.Play();
        }
    }

    private IEnumerator RemoveGameObjectAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(applicationDelay);

        affectedObjects.Remove(obj);
    }
}
