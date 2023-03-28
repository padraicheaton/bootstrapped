using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    public GameObject GenerateWeapon(int[] dna, Vector3 spawnPoint)
    {
        WeaponData weapon = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna);

        GameObject createdWeapon = Instantiate(weapon.prefab, spawnPoint, Quaternion.identity);

        WeaponController weaponController = createdWeapon.GetComponent<WeaponController>();

        weaponController.Construct(dna);

        // Log Weapon Data
        string debug = $"Weapon: {weapon.displayName}, {ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna)} - ";

        foreach (ProjectileModifier m in ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna))
            debug += m.ToString() + ", ";

        Debug.Log(debug);

        return createdWeapon;
    }
}
