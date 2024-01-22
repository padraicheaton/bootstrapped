using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModularProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider physicalCollider;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem projectileParticle;

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
    private int lives = 1;
    private LayerMask affectingLayers;
    private Vector3 desiredScale = Vector3.one;
    private bool shouldTick = false;
    private bool collidedThisFrame = false;

    private float modifierAdditiveDelay;

    private EffectData effectToApply;

    private Collider[] colliders;

    public void Construct(int[] dna, float launchForce, float damage, LayerMask affectingLayers)
    {
        this.dna = dna;
        this.damage = damage;
        this.affectingLayers = affectingLayers;

        rb = GetComponent<Rigidbody>();

        collidableTransform = transform.GetChild(0);

        colliders = GetComponentsInChildren<Collider>();

        modifierAdditiveDelay = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModifierAdditiveDelay(dna);

        foreach (ProjectileModifier m in ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna))
        {
            chosenModifiers.Add((ProjectileModifier)Activator.CreateInstance(m.GetType()));
        }

        effectToApply = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna);

        if (effectToApply.particlePrefab != null)
            Instantiate(effectToApply.particlePrefab, transform);

        rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);

        shouldTick = true;

        ParticleSystem.MainModule main = projectileParticle.main;
        main.startColor = effectToApply.weaponVFXColour;

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

    private void LateUpdate()
    {
        collidedThisFrame = false;
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

        if (onDestroyed != null)
            onDestroyed.Invoke();

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsGameObjectInLayerMask(other.gameObject, affectingLayers))
        {
            // Apply effect, destroy, return
            ExplodeEffect();

            lives--;

            if (lives <= 0)
            {
                if (onDestroyed != null)
                    onDestroyed.Invoke();

                // if (physicalCollider)
                // {
                //     Debug.Log("Disabled physical cllider");
                //     physicalCollider.enabled = false;
                // }
                gameObject.layer = LayerMask.NameToLayer("NoClip");

                Destroy(gameObject);

                return;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collidedThisFrame) return;

        collidedThisFrame = true;

        if (onCollided != null)
            onCollided.Invoke(other);
    }


    private bool IsGameObjectInLayerMask(GameObject obj, LayerMask mask)
    {
        return mask == (mask | (1 << obj.layer));
    }

    public void AddLayerToAffectingLayers(int layer)
    {
        affectingLayers |= (1 << layer);
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

        ShowExplosionEffect();

    }

    private void ShowExplosionEffect()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        ParticleSystem.MainModule main = explosion.GetComponent<ParticleSystem>().main;

        main.startColor = effectToApply.weaponVFXColour;

        // 1.5f = Radius of the trigger for the explosion
        main.startSize = 1.5f * explosionRadiusMultiplier;
    }

    public LayerMask GetAffectingLayers()
    {
        return affectingLayers;
    }

    public void MultiplyProjectileScale(float multiplier)
    {
        MultiplyProjectileScale(Vector3.one * multiplier);
        MultiplyExplosionRadius(multiplier);
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

    public void AddLives(int num)
    {
        lives += num;
    }

    public void DisableColliderForDuration(float delay)
    {
        StartCoroutine(DisableColliderRoutine(delay));
    }

    private IEnumerator DisableColliderRoutine(float delay)
    {
        foreach (Collider c in colliders)
            c.enabled = false;

        yield return new WaitForSeconds(delay);

        foreach (Collider c in colliders)
            c.enabled = true;
    }
}
