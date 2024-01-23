using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private List<SceneGenSettings> settings = new List<SceneGenSettings>();

    private void Start()
    {
        ServiceLocator.instance.GetService<SceneController>().onSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged()
    {

    }

    public GameObject GenerateWeapon(Vector3 spawnPoint)
    {
        int[] weaponDNA = GetSettings().GetNextWeapon(WeaponDataCollector.GetEvolutionaryData());

        Debug.Log($"{GetSettings().name} used to generate");

        return GenerateWeapon(weaponDNA, spawnPoint);
    }

    public GameObject GenerateWeapon(int[] dna, Vector3 spawnPoint)
    {
        WeaponData weapon = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna);

        GameObject createdWeapon = Instantiate(weapon.prefab, spawnPoint, Quaternion.identity);

        WeaponController weaponController = createdWeapon.GetComponent<WeaponController>();

        weaponController.Construct(dna);

        // Log this event
        new EventLogger.Event(
            "Weapon Generated",
            string.Join("-", dna)
        );

        return createdWeapon;
    }

    private BaseGenerationAlgorithm GetSettings()
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
        public BaseGenerationAlgorithm config;
    }
}
