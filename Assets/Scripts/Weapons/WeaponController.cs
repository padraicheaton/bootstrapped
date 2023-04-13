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

    private bool isFireInputPressed;
    private float timeSinceLastFired;
    public int remainingAmmo { get; private set; }

    private Transform weaponFirePoint;

    // Private Component References
    private Rigidbody rb;
    private BoxCollider coll;

    // ! The Important Stuff
    public int[] dna { get; private set; }

    public void Construct(int[] dna)
    {
        this.dna = dna;

        remainingAmmo = clipSize;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isFireInputPressed && timeSinceLastFired >= timeBetweenShots && remainingAmmo > 0)
        {
            Fire();

            timeSinceLastFired = 0f;

            if (!isAutomatic)
                isFireInputPressed = false;
        }

        timeSinceLastFired += Time.deltaTime;
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

        InvokeEvent(WeaponDataCollector.onWeaponEquipped);
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
            InvokeEvent(WeaponDataCollector.onWeaponClipEmptied);
    }

    private Vector3 GetSpreadOffset()
    {
        Vector3 rotationOffset = new Vector3();

        rotationOffset.x = Random.Range(-1f, 1f) * spread;
        rotationOffset.y = Random.Range(-1, 1f) * spread;
        rotationOffset.z = Random.Range(-1f, 1f) * spread;

        return rotationOffset;
    }

    public void ShowWeapon(bool shown)
    {
        IsActive = shown;

        visuals.gameObject.SetActive(shown);
    }

    public void ReloadWeapon()
    {
        remainingAmmo = clipSize;

        InvokeEvent(WeaponDataCollector.onWeaponReloaded);
    }

    private void InvokeEvent(UnityAction<int[]> action)
    {
        // Make this distinction so that weapons used on the shooting range have no bearing on the preference selection
        if (action != null && ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == LoadedScenes.Sandbox)
            action.Invoke(dna);
    }
}
