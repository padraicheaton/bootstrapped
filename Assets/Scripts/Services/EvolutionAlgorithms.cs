using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionAlgorithms
{
    public static int[] Randomised()
    {
        // The core of this function remains in the component provider as it requires access to the total length of each component
        return ServiceLocator.instance.GetService<WeaponComponentProvider>().GetRandomDNA();
    }

    public static int[] Crossover(float mutationRate)
    {
        return Crossover(WeaponDataCollector.GetFittestParents(), mutationRate);
    }

    public static int[] Crossover(int[][] parents, float mutationRate)
    {
        List<int> childGenome = new List<int>();

        // Cross over the discrete genome characteristics: weapon, effect, additive delay, modifier count
        for (int i = 0; i < 4; i++)
        {
            if (Random.value < mutationRate)
                childGenome.Add(ServiceLocator.instance.GetService<WeaponComponentProvider>().GetRandomDiscreteAllele(i));
            else
                childGenome.Add(GetRandomAlleleFromPool(parents, i));
        }

        // Add in modifiers - at this point, the last index of the genome is the amount of modifiers to add
        int numOfModifiers = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModiferCountOption(childGenome.ToArray());
        childGenome.AddRange(GetRandomModifiersFromPool(parents, numOfModifiers, mutationRate));

        return childGenome.ToArray();
    }

    private static int GetRandomAlleleFromPool(int[][] populationPool, int index)
    {
        int[] selectedParent = populationPool[Random.Range(0, populationPool.Length)];

        if (selectedParent.Length > index)
            return selectedParent[index];
        else
        {
            Debug.LogError("Index Out of Range!");
            return 0;
        }
    }

    private static int[] GetRandomModifiersFromPool(int[][] populationPool, int amountOfModifiers, float mutationChance)
    {
        List<int> modifierDNA = new List<int>();

        for (int i = 0; i < amountOfModifiers; i++)
        {
            if (Random.value < mutationChance)
                modifierDNA.Add(ServiceLocator.instance.GetService<WeaponComponentProvider>().GetRandomModifier());
            else
            {
                int[] selectedParent = populationPool[Random.Range(0, populationPool.Length)];

                // The first index that holds modifiers is 4
                int modifier = selectedParent[Random.Range(4, selectedParent.Length)];

                modifierDNA.Add(modifier);
            }
        }

        return modifierDNA.ToArray();
    }
}
