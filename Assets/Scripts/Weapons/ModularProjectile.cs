using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModularProjectile : MonoBehaviour
{
    private Rigidbody rb;

    private List<ProjectileModifier> chosenModifiers = new List<ProjectileModifier>();
    private List<ProjectileModifier> activeModifiers = new List<ProjectileModifier>();

    public UnityAction<Collision> onCollided;
    public UnityAction onDestroyed;

    private int[] dna;

    private Transform collidableTransform;
    private float explosionRadiusMultiplier = 1f;
    private float damage;
    private float timeToLive = 3f;
    private LayerMask affectingLayers;
    private Vector3 desiredScale = Vector3.one;
    private bool shouldTick = false;

    private float modifierAdditiveDelay;

    private EffectData effectToApply;

    public void Construct(int[] dna, float launchForce, float damage, LayerMask affectingLayers)
    {
        this.dna = dna;
        this.damage = damage;
        this.affectingLayers = affectingLayers;

        rb = GetComponent<Rigidbody>();

        collidableTransform = transform.GetChild(0);

        modifierAdditiveDelay = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModifierAdditiveDelay(dna);

        chosenModifiers.AddRange(ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna));

        effectToApply = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna);

        rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);

        shouldTick = true;

        StartCoroutine(ModifierApplicationRoutine());
    }

    private void Update()
    {
        foreach (ProjectileModifier m in activeModifiers)
        {
            if (shouldTick && m.IsValid())
                m.TickModifier(Time.deltaTime);
        }

        // Scale the child to the desired value
        collidableTransform.localScale = Vector3.Lerp(collidableTransform.localScale, desiredScale, Time.deltaTime * 25f);
    }

    private IEnumerator ModifierApplicationRoutine()
    {
        foreach (ProjectileModifier m in chosenModifiers)
        {
            yield return new WaitForSeconds(modifierAdditiveDelay);

            m.SetupModifier(rb, collidableTransform, this);

            activeModifiers.Add(m);
        }

        yield return new WaitForSeconds(timeToLive);

        ExplodeEffect();

        Destroy(gameObject);
    }


    private bool IsGameObjectInLayerMask(GameObject obj, LayerMask mask)
    {
        return mask == (mask | (1 << obj.layer));
    }

    private void ExplodeEffect()
    {
        activeModifiers.Clear();
        StopAllCoroutines();

        // Explode in an area multiplier * the size of the original collider
        foreach (Collider coll in Physics.OverlapBox(collidableTransform.position, collidableTransform.localScale * explosionRadiusMultiplier, collidableTransform.transform.rotation, affectingLayers))
        {
            if (coll.TryGetComponent<HealthComponent>(out HealthComponent hc))
            {
                BaseEffect effectScript = Instantiate(effectToApply.prefab, coll.transform).GetComponent<BaseEffect>();
                effectScript.OnEffectApplied(hc, damage, collidableTransform.gameObject); // passing the child here as this ref is used for force calculation
            }
        }
    }

    public LayerMask GetAffectingLayers()
    {
        return affectingLayers;
    }

    public void MultiplyProjectileScale(float multiplier)
    {
        MultiplyProjectileScale(Vector3.one * multiplier);
    }

    public void MultiplyProjectileScale(Vector3 multiplier)
    {
        desiredScale.x *= multiplier.x;
        desiredScale.y *= multiplier.y;
        desiredScale.z *= multiplier.z;
    }

    public void MultiplyExplosionRadius(float multiplier)
    {
        explosionRadiusMultiplier *= multiplier;
    }

    public void MultiplyDamageAmount(float multiplier)
    {
        damage *= multiplier;
    }

    public void MultiplyTimeToLive(float multiplier)
    {
        timeToLive *= multiplier;
    }
}
