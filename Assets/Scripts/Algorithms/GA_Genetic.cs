using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Genetic Gen", menuName = "Bootstrapped/Gen Obj/Genetic")]
public class GA_Genetic : BaseGenerationAlgorithm
{
    [Range(0f, 1f)] public float mutationRate;
    public int minimumDataPoints;
    [Range(0f, 1f)] public float noveltyChance;
    [Range(0f, 1f)] public float noveltyChanceIncrement;

    private float activeNoveltyChance;

    public override void Initialize()
    {
        activeNoveltyChance = noveltyChance;
    }

    public override int[] GetNextWeapon(EvolutionaryData[] evolutionaryData)
    {
        if (evolutionaryData.Length < minimumDataPoints)
            return EvolutionAlgorithms.Randomised();

        if (activeNoveltyChance > 0f && Random.value <= activeNoveltyChance)
        {
            activeNoveltyChance = noveltyChance;

            return EvolutionAlgorithms.Randomised();
        }
        else
        {
            activeNoveltyChance += noveltyChanceIncrement;

            return ConstructWeaponDNA(evolutionaryData);
        }
    }

    protected virtual int[] ConstructWeaponDNA(EvolutionaryData[] evolutionaryData)
    {
        return EvolutionAlgorithms.Crossover(mutationRate);
    }
}
