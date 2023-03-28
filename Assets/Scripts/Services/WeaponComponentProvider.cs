using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using Random = UnityEngine.Random;

public class WeaponComponentProvider : MonoBehaviour
{
    private ProjectileModifier[] modifiers;
    private EffectData[] effects;
    private WeaponData[] weapons;

    private float[] modifierAdditiveDelays = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
    private int[] modifierCountOptions = new int[] { 1, 2, 3, 4 };

    private void Awake()
    {
        LoadAllProjectileModifiers();

        effects = GetAllScriptableObjects<EffectData>();
        weapons = GetAllScriptableObjects<WeaponData>();

        Debug.Log(effects.Length);
        Debug.Log(weapons.Length);
    }

    private void LoadAllProjectileModifiers()
    {
        List<ProjectileModifier> foundModifiers = new List<ProjectileModifier>();

        foreach (Type cls in Assembly.GetAssembly(typeof(ProjectileModifier)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ProjectileModifier))))
        {
            foundModifiers.Add(Activator.CreateInstance(cls) as ProjectileModifier);
        }

        modifiers = foundModifiers.ToArray();
    }

    private T[] GetAllScriptableObjects<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("");
    }

    public WeaponData GetWeaponObject(int[] dna)
    {
        // Index 0 of the dna corresponds to the weapon type
        int index = dna[0];

        return weapons[index];
    }

    public EffectData GetEffectObject(int[] dna)
    {
        // Index 1 of the dna corresponds to the effect type
        int index = dna[1];

        return effects[index];
    }

    public float GetModifierAdditiveDelay(int[] dna)
    {
        // Index 2 of the dna corresponds to the modifier additive delay
        int index = dna[2];

        return modifierAdditiveDelays[index];
    }

    public int GetModiferCountOption(int[] dna)
    {
        // Index 3 of the dna corresponds to the amount of modifiers applied to a projectile
        int index = dna[3];

        return modifierCountOptions[index];
    }

    public ProjectileModifier[] GetProjectileModifiers(int[] dna)
    {
        int numOfModifiers = GetModiferCountOption(dna);

        ProjectileModifier[] selectedModifiers = new ProjectileModifier[numOfModifiers];

        for (int i = 0; i < numOfModifiers; i++)
        {
            int modifierIndex = dna[4 + i];

            selectedModifiers[i] = modifiers[modifierIndex];
        }

        return selectedModifiers;
    }

    public int[] GetRandomDNA()
    {
        List<int> randomGenome = new List<int>();

        randomGenome.Add(Random.Range(0, weapons.Length));
        randomGenome.Add(Random.Range(0, effects.Length));
        randomGenome.Add(Random.Range(0, modifierAdditiveDelays.Length));
        randomGenome.Add(Random.Range(0, modifierCountOptions.Length));

        for (int i = 0; i < GetModiferCountOption(randomGenome.ToArray()); i++)
            randomGenome.Add(Random.Range(0, modifiers.Length));

        return randomGenome.ToArray();
    }
}
