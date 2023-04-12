using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // Load the currency
        SaveStateController.LoadSaveData();

        CurrencyHandler.Setup();
    }

    private void Start()
    {
        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == LoadedScenes.Sandbox)
        {
            UpgradeLoader.ApplyUpgradesToPlayer();
        }

        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MainMenu)
            ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealth().onDeath += OnGameOver;
    }

    private void OnGameOver()
    {
        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(LoadedScenes.Lab);
    }
}
