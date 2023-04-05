using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealth().onDeath += OnGameOver;

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == LoadedScenes.Sandbox)
        {
            UpgradeLoader.ApplyUpgradesToPlayer();
        }
    }

    private void OnGameOver()
    {
        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(LoadedScenes.Lab);
    }
}
