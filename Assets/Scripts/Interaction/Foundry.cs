using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundry : Interactable
{
    [Header("Settings")]
    [SerializeField] private int weaponCost;

    public override string GetName()
    {
        return $"Weapon Foundry\nCost: {weaponCost}";
    }

    private void Update()
    {
        IsInteractable = !ServiceLocator.instance.GetService<Spawner>().swarmInProgress;
    }

    public override void OnInteracted()
    {
        if (CurrencyHandler.CanAfford(weaponCost))
        {
            GameObject generatedWeapon = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(transform.position + Vector3.up);

            generatedWeapon.GetComponent<Rigidbody>().AddForce((Random.insideUnitSphere + Vector3.up) * 5f, ForceMode.Impulse);

            CurrencyHandler.DecreaseSparePartCount(weaponCost);
        }
    }
}
