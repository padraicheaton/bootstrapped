using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private List<SceneGenSettings> settings = new List<SceneGenSettings>();

    private float cachedNoveltyChance;

    private void Start()
    {
        cachedNoveltyChance = GetSettings().noveltyChance;

        ServiceLocator.instance.GetService<SceneController>().onSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged()
    {
        cachedNoveltyChance = GetSettings().noveltyChance;
    }

    public GameObject GenerateWeapon(Vector3 spawnPoint, bool forceRandom = false)
    {
        if (Random.value < GetSettings().noveltyChance || forceRandom)
        {
            // Reset the novelty chance back to default
            GetSettings().noveltyChance = cachedNoveltyChance;

            return GenerateWeapon(EvolutionAlgorithms.Randomised(), spawnPoint);
        }
        else
        {
            // Increment the novelty chance if switched on
            if (GetSettings().shouldIncrementNovelty)
                GetSettings().noveltyChance += GetSettings().noveltyChanceIncrement;

            return GenerateWeapon(EvolutionAlgorithms.Crossover(GetSettings().mutationChance), spawnPoint);
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

    private GenConfig GetSettings()
    {
        LoadedScenes activeScene = ServiceLocator.instance.GetService<SceneController>().GetActiveScene();

        foreach (SceneGenSettings s in settings)
        {
            if (s.scene == activeScene)
                return s.config;
        }

        if (settings.Count > 0)
            return settings[0].config;
        else
            return null;
    }

    [System.Serializable]
    public struct SceneGenSettings
    {
        public LoadedScenes scene;
        public GenConfig config;
    }
}
