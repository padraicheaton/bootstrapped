using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class WeaponDataCollector
{
    private static List<EvolutionaryData> evolutionaryData = new List<EvolutionaryData>();

    public static UnityAction<int[]> onWeaponEquipped;
    public static UnityAction<int[]> onWeaponClipEmptied;
    public static UnityAction<int[]> onWeaponReloaded;
    public static UnityAction<int[]> onEnemyKilled;

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
            if (data.dna == dna)
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
    }

    public static EvolutionaryData[] GetEvolutionaryData()
    {
        return evolutionaryData.ToArray();
    }

    public static int[][] GetFittestParents(int numParents = 2)
    {
        int[][] parentsPopulation = new int[numParents][];

        for (int i = 0; i < numParents; i++)
        {
            if (i + 1 > evolutionaryData.Count)
                parentsPopulation[i] = EvolutionAlgorithms.Randomised();
            else
                parentsPopulation[i] = GetMaxFitParent(parentsPopulation);

            Debug.Log($"Selected Parent {string.Join(", ", parentsPopulation[i])}");
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
            if (!selectedParents.Contains(data.dna) && data.GetFitness() > maxFitness)
            {
                maxFitness = data.GetFitness();
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

    public EvolutionaryData(int[] dna)
    {
        this.dna = dna;

        timesClipEmptied = 0;
        killsDealt = 0;
        timesReloaded = 0;
    }

    public float GetFitness()
    {
        return timesClipEmptied + killsDealt + timesReloaded;
    }
}
