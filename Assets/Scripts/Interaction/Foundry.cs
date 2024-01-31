using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Foundry : Interactable
{
    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject boxObj;
    [SerializeField] private Collider interactinCollider;

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

    private void Start()
    {
        remainingWeaponsToGenerate = numWeaponsToGenerateEachWave;

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += () =>
        {
            if (numWeaponsToGenerateEachWave < 20)
                numWeaponsToGenerateEachWave += additiveIncrease;
            remainingWeaponsToGenerate = numWeaponsToGenerateEachWave;

            SetVisibility(true);
        };

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

        SetVisibility(false);

        new EventLogger.Event("Interacted With Foundry",
                                $"{numWeaponsToGenerateEachWave} Weapons To Generate");

        for (int i = 0; i < numWeaponsToGenerateEachWave; i++)
        {
            GameObject generatedWeapon = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(spawnPoint.position);

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
            if (wep != null && wep.transform.parent == null)
                Destroy(wep.gameObject);
        }

        spawnedWeapons.Clear();
    }

    private void SetVisibility(bool visible)
    {
        boxObj.SetActive(visible);
        interactinCollider.enabled = visible;
    }
}
