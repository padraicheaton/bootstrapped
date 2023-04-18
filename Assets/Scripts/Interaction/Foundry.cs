using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundry : PhaseBasedInteractable
{
    [Header("Settings")]
    [SerializeField] private int numWeaponsToGenerateEachWave;
    [SerializeField] private int additiveIncrease;

    private int remainingWeaponsToGenerate;

    private bool currentlySpawningWeapons = false;

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

            yield return new WaitForSeconds(0.25f);
        }

        currentlySpawningWeapons = false;
    }
}
