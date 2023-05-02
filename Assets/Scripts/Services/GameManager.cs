using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // Load the data
        SaveStateController.LoadSaveData();

        CurrencyHandler.Setup();
        WeaponDataCollector.RegisterEventListeners();
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void RegisterEventListeners()
    {
        UpgradeLoader.ApplyUpgradesToPlayer();

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MainMenu)
            ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealth().onDeath += OnGameOver;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        RegisterEventListeners();

        ServiceLocator.instance.GetService<SceneController>().onSceneChanged += RegisterEventListeners;
    }

    private void OnGameOver()
    {
        // If in the sandbox, go back to the lab
        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == LoadedScenes.Sandbox)
            ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(LoadedScenes.Lab);
        // Else, reload the current scene
        else
            ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(ServiceLocator.instance.GetService<SceneController>().GetActiveScene());
    }

    private void OnApplicationQuit()
    {
        SaveStateController.SaveDataToFile();
    }
}
