using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Range(0f, 1f)] private float noveltyChance;
    [SerializeField] private bool shouldIncrementNovelty;
    [SerializeField] private float noveltyChanceIncrement;
    [SerializeField][Range(0f, 1f)] private float mutationChance;

    private float cachedNoveltyChance;

    private void Start()
    {
        cachedNoveltyChance = noveltyChance;
    }

    public GameObject GenerateWeapon(Vector3 spawnPoint)
    {
        if (Random.value < noveltyChance)
        {
            // Reset the novelty chance back to default
            noveltyChance = cachedNoveltyChance;

            return GenerateWeapon(EvolutionAlgorithms.Randomised(), spawnPoint);
        }
        else
        {
            // Increment the novelty chance if switched on
            if (shouldIncrementNovelty)
                noveltyChance += noveltyChanceIncrement;

            return GenerateWeapon(EvolutionAlgorithms.Crossover(mutationChance), spawnPoint);
        }
    }

    public GameObject GenerateWeapon(int[] dna, Vector3 spawnPoint)
    {
        WeaponData weapon = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna);

        GameObject createdWeapon = Instantiate(weapon.prefab, spawnPoint, Quaternion.identity);

        WeaponController weaponController = createdWeapon.GetComponent<WeaponController>();

        weaponController.Construct(dna);

        return createdWeapon;
    }
}
