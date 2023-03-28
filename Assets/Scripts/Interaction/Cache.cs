using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : Interactable
{
    public override string GetName()
    {
        return "Weapon Cache";
    }

    public override void OnInteracted()
    {
        int[] dna = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetRandomDNA();

        GameObject generatedWeapon = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(dna, transform.position + Vector3.up * 0.5f);

        generatedWeapon.GetComponent<Rigidbody>().AddForce((Random.onUnitSphere + Vector3.up) * 5f, ForceMode.Impulse);
    }
}
