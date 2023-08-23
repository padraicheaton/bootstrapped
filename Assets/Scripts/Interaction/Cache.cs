using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : Interactable
{
    [Header("Settings")]
    [SerializeField] private int weaponsToDrop;
    [SerializeField] private bool destroyOnOpen;

    public override string GetName()
    {
        return "Weapon Cache";
    }

    public override void OnInteracted()
    {
        for (int i = 0; i < weaponsToDrop; i++)
        {
            GameObject generatedWeapon = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(transform.position + Vector3.up);

            generatedWeapon.GetComponent<Rigidbody>().AddForce((Random.insideUnitSphere + Vector3.up) * 5f, ForceMode.Impulse);
        }

        if (destroyOnOpen)
            Destroy(gameObject);
    }
}
