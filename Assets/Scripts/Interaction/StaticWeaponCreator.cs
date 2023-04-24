using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWeaponCreator : MonoBehaviour
{
    [SerializeField] public List<StaticWeapon> weapons = new List<StaticWeapon>();

    private void Start()
    {
        foreach (StaticWeapon dna in weapons)
            ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(dna.genotype, transform.position);
    }

    [System.Serializable]
    public class StaticWeapon
    {
        public int[] genotype;

        public StaticWeapon()
        {
            genotype = new int[] { 1, 0, 0, 0, 0 };
        }
    }
}