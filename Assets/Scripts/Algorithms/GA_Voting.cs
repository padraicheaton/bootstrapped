using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Voting Gen", menuName = "Bootstrapped/Gen Obj/Voting")]
public class GA_Voting : GA_Genetic
{
    protected override int[] ConstructWeaponDNA(EvolutionaryData[] evolutionaryData)
    {
        if (evolutionaryData.Length < minimumDataPoints)
            return EvolutionAlgorithms.Randomised();

        // For each component, add each allele to a single array, pick randomly from that array
        // It will be naturally biased towards the more popular alleles
        // Repeat until the modifier alleles

        // Example DNA Structure = {Weapon Type, Effect Type, Additive Delay, Modifier Count, Mod, Mod,...}
        List<int> electedDna = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            electedDna.Add(GetBiasedAllele(i, evolutionaryData));
        }

        int amountOfModifiers = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModiferCountOption(electedDna.ToArray());

        for (int i = 0; i < amountOfModifiers; i++)
        {
            electedDna.Add(GetBiasedModifierAllele(evolutionaryData));
        }

        string strRepresentation = string.Join(",", electedDna.ToArray());
        Debug.Log($"Elected DNA: {strRepresentation}");

        return electedDna.ToArray();
    }

    private int GetBiasedAllele(int dnaIndex, EvolutionaryData[] evolutionaryData)
    {
        List<int> existingAlleles = new List<int>();

        foreach (EvolutionaryData data in evolutionaryData)
            existingAlleles.Add(data.dna[dnaIndex]);

        return existingAlleles[Random.Range(0, existingAlleles.Count)];
    }

    private int GetBiasedModifierAllele(EvolutionaryData[] evolutionaryData)
    {
        List<int> existingAlleles = new List<int>();

        foreach (EvolutionaryData data in evolutionaryData)
            // The first index where modifiers are present is 4
            for (int i = 4; i < data.dna.Length; i++)
                existingAlleles.Add(data.dna[i]);

        return existingAlleles[Random.Range(0, existingAlleles.Count)];
    }
}
