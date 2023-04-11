using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWeaponCreator : MonoBehaviour
{
    public int[] dna;

    private void Start()
    {
        ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(dna, transform.position);
    }
}
