using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : Interactable
{
    private bool IsActive;

    [Header("References")]
    [SerializeField] private Transform visuals;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Settings")]
    [SerializeField] private string displayName;
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float initialLaunchForce;
    [SerializeField] private int clipSize;
    [SerializeField] private bool isAutomatic;

    private bool isFireInputPressed;
    private float timeSinceLastFired;

    private Transform weaponFirePoint;

    // Private Component References
    private Rigidbody rb;
    private BoxCollider coll;

    public void Construct(int[] dna)
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isFireInputPressed && timeSinceLastFired >= timeBetweenShots)
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
    }

    public void OnDropped()
    {
        rb.isKinematic = false;
        coll.enabled = true;
    }

    public void OnFireInput(bool performed)
    {
        isFireInputPressed = performed;
    }

    private void Fire()
    {
        if (!weaponFirePoint) return;

        Instantiate(projectilePrefab, weaponFirePoint.position, weaponFirePoint.rotation);
    }

    public void ShowWeapon(bool shown)
    {
        IsActive = shown;

        visuals.gameObject.SetActive(shown);
    }
}
