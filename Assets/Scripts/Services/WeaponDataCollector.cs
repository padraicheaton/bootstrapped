using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public static class WeaponDataCollector
{
    private static List<EvolutionaryData> evolutionaryData = new List<EvolutionaryData>();

    public static UnityAction<int[]> onWeaponEquipped;
    public static UnityAction<int[]> onWeaponClipEmptied;
    public static UnityAction<int[]> onWeaponReloaded;
    public static UnityAction<int[]> onEnemyKilled;

    public static readonly int[] defaultStartingWeapon = new int[] { 1, 0, 0, 0, 0 };

    public static void RegisterEventListeners()
    {
        onWeaponEquipped += dna =>
        {
            // This function creates the object if it does not exist
            GetEvolutionaryData(dna);
            SaveStateController.SaveEvolutionaryDataToFile();
        };

        onWeaponReloaded += dna =>
        {
            GetEvolutionaryData(dna).timesReloaded++;
            SaveStateController.SaveEvolutionaryDataToFile();
        };

        onEnemyKilled += dna =>
        {
            GetEvolutionaryData(dna).killsDealt++;
            SaveStateController.SaveEvolutionaryDataToFile();
        };

        onWeaponClipEmptied += dna =>
        {
            GetEvolutionaryData(dna).timesClipEmptied++;
            SaveStateController.SaveEvolutionaryDataToFile();
        };
    }

    private static EvolutionaryData GetEvolutionaryData(int[] dna)
    {
        foreach (EvolutionaryData data in evolutionaryData)
        {
            if (data.dna.SequenceEqual(dna))
                return data;
        }

        EvolutionaryData newData = new EvolutionaryData(dna);

        evolutionaryData.Add(newData);

        return newData;
    }

    public static void LoadEvolutionaryData(EvolutionaryData[] serialisedData)
    {
        evolutionaryData = new List<EvolutionaryData>();

        evolutionaryData.AddRange(serialisedData);

        foreach (EvolutionaryData d in evolutionaryData)
            Debug.Log($"Loaded weapon with {d.timesClipEmptied} times clip emptied");
    }

    public static EvolutionaryData[] GetEvolutionaryData()
    {
        return evolutionaryData.ToArray();
    }

    public static int[] GetStartingWeaponGenotype()
    {
        foreach (EvolutionaryData data in evolutionaryData)
        {
            if (data.isStartingWeapon)
                return data.dna;
        }

        return defaultStartingWeapon;
    }

    public static void SetStartingWeapon(int[] dna)
    {
        // Creates the data if not exists
        GetEvolutionaryData(dna);

        foreach (EvolutionaryData data in evolutionaryData)
        {
            data.isStartingWeapon = data.dna == dna;
        }
    }

    public static int[][] GetFittestParents(int numParents = 2)
    {
        Debug.Log($"Population Count {evolutionaryData.Count}");

        int[][] parentsPopulation = new int[numParents][];

        for (int i = 0; i < numParents; i++)
        {
            if (i + 1 > evolutionaryData.Count)
                parentsPopulation[i] = EvolutionAlgorithms.Randomised();
            else
                parentsPopulation[i] = GetMaxFitParent(parentsPopulation);
        }

        return parentsPopulation;
    }

    private static int[] GetMaxFitParent(int[][] alreadySelectedParents)
    {
        List<int[]> selectedParents = new List<int[]>();
        selectedParents.AddRange(alreadySelectedParents);

        float maxFitness = float.MinValue;
        int[] dna = null;

        foreach (EvolutionaryData data in evolutionaryData)
        {
            Debug.Log($"Calculated Fitness of {string.Join(", ", data.dna)} = {data.GetFitness(evolutionaryData)}");

            if (!selectedParents.Contains(data.dna) && data.GetFitness(evolutionaryData) > maxFitness)
            {
                maxFitness = data.GetFitness(evolutionaryData);
                dna = data.dna;
            }
        }

        if (dna == null)
        {
            Debug.LogError("NO VALID DNA FOUND");
            return evolutionaryData[0].dna;
        }

        return dna;
    }
}

[System.Serializable]
public class EvolutionaryData
{
    public int[] dna;

    public int timesClipEmptied;
    public int killsDealt;
    public int timesReloaded;

    public bool isStartingWeapon;

    public EvolutionaryData(int[] dna)
    {
        this.dna = dna;

        timesClipEmptied = 0;
        killsDealt = 0;
        timesReloaded = 0;
        isStartingWeapon = false;
    }

    public float GetFitness(List<EvolutionaryData> populationPool)
    {
        float normalised_timesClipEmptied = GetNormalisedFieldValue(populationPool, "timesClipEmptied", timesClipEmptied);
        float normalised_killsDealt = GetNormalisedFieldValue(populationPool, "killsDealt", killsDealt);
        float normalised_timesReloaded = GetNormalisedFieldValue(populationPool, "timesReloaded", timesReloaded);

        float normalised_isStartingWeapon = isStartingWeapon ? 1f : 0f;

        //Debug.Log($"Fitness: {timesClipEmptied} + {killsDealt} + {timesReloaded} + {isStartingWeapon}");
        //Debug.Log($"Fitness: {normalised_timesClipEmptied} + {normalised_killsDealt} + {normalised_timesReloaded} + {normalised_isStartingWeapon}");

        return normalised_timesClipEmptied +
                normalised_killsDealt +
                normalised_timesReloaded +
                normalised_isStartingWeapon;
    }

    private float GetNormalisedFieldValue(List<EvolutionaryData> classList, string attributeName, int value)
    {
        int maxAttributeValue = int.MinValue; // Initialize to smallest possible integer value
        foreach (EvolutionaryData c in classList)
        {
            int attributeValue = (int)c.GetType().GetField(attributeName).GetValue(c);
            //int attributeValue = (int)c.GetType().GetProperty(attributeName).GetValue(c, null);
            if (attributeValue > maxAttributeValue)
            {
                maxAttributeValue = attributeValue;
            }
        }

        if (maxAttributeValue == 0)
            return 0;
        else
            return (float)value / (float)maxAttributeValue;
    }
}
