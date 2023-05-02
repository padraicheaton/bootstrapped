using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Foundry : PhaseBasedInteractable
{
    [Header("Settings")]
    [SerializeField] private int numWeaponsToGenerateEachWave;
    [SerializeField] private int additiveIncrease;

    private int remainingWeaponsToGenerate;

    private bool currentlySpawningWeapons = false;

    private List<WeaponController> spawnedWeapons = new List<WeaponController>();

    public override string GetName()
    {
        return $"Weapon Foundry\n({remainingWeaponsToGenerate})";
    }

    protected override void Start()
    {
        base.Start();

        remainingWeaponsToGenerate = numWeaponsToGenerateEachWave;

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += () =>
        {
            remainingWeaponsToGenerate = numWeaponsToGenerateEachWave;

            numWeaponsToGenerateEachWave += additiveIncrease;
        };

        WeaponDataCollector.onWeaponEquipped += UntrackSpawnedWeapon;

        ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += DestroyUnusedWeapons;
    }

    public override void OnInteracted()
    {
        if (!currentlySpawningWeapons && remainingWeaponsToGenerate > 0)
            StartCoroutine(SpawnRemainingWeapons());
    }

    private IEnumerator SpawnRemainingWeapons()
    {
        currentlySpawningWeapons = true;

        for (int i = 0; i < numWeaponsToGenerateEachWave; i++)
        {
            GameObject generatedWeapon = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(transform.position + Vector3.up);

            Rigidbody rb = generatedWeapon.GetComponent<Rigidbody>();
            rb.AddForce((Random.onUnitSphere + Vector3.up) * 5f, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * 5f);

            remainingWeaponsToGenerate--;

            spawnedWeapons.Add(generatedWeapon.GetComponent<WeaponController>());

            yield return new WaitForSeconds(0.2f);
        }

        currentlySpawningWeapons = false;
    }

    private void DestroyUnusedWeapons()
    {
        foreach (WeaponController wep in spawnedWeapons)
        {
            Destroy(wep.gameObject);
        }
    }

    private void UntrackSpawnedWeapon(int[] dna)
    {
        List<WeaponController> toRemove = new List<WeaponController>();

        foreach (WeaponController wep in spawnedWeapons)
        {
            //! BANDAID FIX - Only equipped weapons are parented to the weapon container
            if (wep.isEquipped || wep.transform.parent != null)
            {
                toRemove.Add(wep);
            }
        }

        foreach (WeaponController rem in toRemove)
            spawnedWeapons.Remove(rem);
    }
}
