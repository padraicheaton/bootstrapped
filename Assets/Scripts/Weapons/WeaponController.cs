using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : Interactable
{
    private bool IsActive;

    [Header("References")]
    [SerializeField] private Transform visuals;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Settings")]
    [SerializeField] private string displayName;
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float initialLaunchForce;
    [SerializeField] private int pelletCount;
    [SerializeField] private int clipSize;
    [SerializeField] private float spread;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private float timeToReload;

    private bool isFireInputPressed;
    private float timeSinceLastFired;
    public int remainingAmmo { get; private set; }
    public float ammoCharge { get { return (float)remainingAmmo / (float)clipSize; } private set { ammoCharge = value; } }
    public bool isRecharging { get; private set; }

    private Transform weaponFirePoint;

    // Private Component References
    private Rigidbody rb;
    private BoxCollider coll;

    private float despawnDelay = 90f;

    // ! The Important Stuff
    public int[] dna { get; private set; }

    public bool isEquipped { get; private set; } = false;

    public void Construct(int[] dna)
    {
        this.dna = dna;

        remainingAmmo = clipSize;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();

        InitiateDespawnTimer();
    }

    private void Update()
    {
        if (CanShoot())
        {
            Fire();

            timeSinceLastFired = 0f;

            if (!isAutomatic)
                isFireInputPressed = false;
        }

        timeSinceLastFired += Time.deltaTime;
    }

    private bool CanShoot()
    {
        return isFireInputPressed && timeSinceLastFired >= timeBetweenShots && remainingAmmo > 0 && !isRecharging;
    }

    public override void OnInteracted()
    {
        ServiceLocator.instance.GetService<PlayerWeaponSystem>().AddWeapon(gameObject);
    }

    public override string GetName()
    {
        return displayName;
    }

    public void OnPickedUp(Transform weaponFirePoint)
    {
        this.weaponFirePoint = weaponFirePoint;

        rb.isKinematic = true;
        coll.enabled = false;

        // Stop despawn timer
        StopAllCoroutines();

        InvokeEvent(WeaponDataCollector.onWeaponEquipped);

        isEquipped = true;
    }

    public void OnDropped()
    {
        rb.isKinematic = false;
        coll.enabled = true;

        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);

        if (remainingAmmo <= 0)
        {
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            IsInteractable = false;
            Destroy(gameObject, 3f);
        }
        else
            InitiateDespawnTimer();

        isEquipped = false;
    }

    public void OnFireInput(bool performed)
    {
        isFireInputPressed = performed;
    }

    private void Fire()
    {
        if (!weaponFirePoint) return;

        for (int i = 0; i < pelletCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, weaponFirePoint.position, Quaternion.Euler(GetSpreadOffset() + weaponFirePoint.eulerAngles));

            projectile.GetComponent<ModularProjectile>().Construct(dna, initialLaunchForce, damage, enemyLayer);
        }

        remainingAmmo--;

        if (remainingAmmo <= 0)
            OnWeaponClipEmptied();
    }

    private Vector3 GetSpreadOffset()
    {
        Vector3 rotationOffset = new Vector3();

        rotationOffset.x = Random.Range(-1f, 1f) * spread;
        rotationOffset.y = Random.Range(-1, 1f) * spread;
        rotationOffset.z = Random.Range(-1f, 1f) * spread;

        return rotationOffset;
    }

    private void OnWeaponClipEmptied()
    {
        InvokeEvent(WeaponDataCollector.onWeaponClipEmptied);

        StartCoroutine(ReloadAfterDelay());
    }

    private IEnumerator ReloadAfterDelay()
    {
        isRecharging = true;

        float progressTimer = 0f;

        while (progressTimer < timeToReload)
        {
            progressTimer += Time.deltaTime;

            remainingAmmo = Mathf.RoundToInt(Mathf.Lerp(0f, clipSize, progressTimer / timeToReload));

            yield return new WaitForEndOfFrame();
        }

        // Not using existing reload function as I do not want to invoke the reload event every time
        remainingAmmo = clipSize;

        isRecharging = false;
    }

    private void InitiateDespawnTimer()
    {
        StopAllCoroutines();
        StartCoroutine(DespawnTimer());
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnDelay);

        Destroy(gameObject);
    }

    public void ShowWeapon(bool shown)
    {
        IsActive = shown;

        visuals.gameObject.SetActive(shown);
    }

    public void ReloadWeapon()
    {
        // if the weapon is actively recharging when reloaded, stop the routine
        StopAllCoroutines();

        remainingAmmo = clipSize;

        isRecharging = false;

        InvokeEvent(WeaponDataCollector.onWeaponReloaded);
    }

    private void InvokeEvent(UnityAction<int[]> action)
    {
        // Make this distinction so that weapons used on the shooting range have no bearing on the preference selection
        if (action != null && ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.Lab)
            action.Invoke(dna);
    }
}
