using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Experiment Settings")]
    [SerializeField] private float timeInSandboxBeforeQuestionnaire;
    private float timeInSandbox;
    private bool isInSandbox;

    public static float totalTimeInSandbox;
    public static string participantID = "Unknown";


    private void Awake()
    {
        // Load the data
        SaveStateController.LoadSaveData();

        CurrencyHandler.Setup();
        WeaponDataCollector.RegisterEventListeners();
    }

    private void OnSceneChanged()
    {
        LoadedScenes activeScene = ServiceLocator.instance.GetService<SceneController>().GetActiveScene();

        isInSandbox = activeScene == LoadedScenes.Sandbox || activeScene == LoadedScenes.Sandbox_Random;
    }

    public void SetParticipantID(string id)
    {
        GameManager.participantID = id;

        new EventLogger.Event("Participant ID Changed", GameManager.participantID);
    }

    private void Update()
    {
        if (isInSandbox)
        {
            totalTimeInSandbox += Time.deltaTime;
            timeInSandbox += Time.deltaTime;

            if (timeInSandbox >= timeInSandboxBeforeQuestionnaire)
            {
                timeInSandbox = 0f;

                ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(LoadedScenes.MidExperimentQuestionnaire);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void RegisterEventListeners()
    {
        UpgradeLoader.ApplyUpgradesToPlayer();

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MainMenu &&
            ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MidExperimentQuestionnaire)
            ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealth().onDeath += OnGameOver;

        ServiceLocator.instance.GetService<SceneController>().onSceneChanged += OnSceneChanged;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        totalTimeInSandbox = timeInSandbox = 0f;

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
