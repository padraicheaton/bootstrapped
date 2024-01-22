using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Experiment Settings")]
    [SerializeField] private float playSessionIntervalMinutes;
    public static string participantID = "Unknown";
    public static bool playerPresentedWithQuestionnaire = false;

    private Coroutine questionnaireTimerRoutine;


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

        if (activeScene == LoadedScenes.Sandbox || activeScene == LoadedScenes.Sandbox_Random)
        {
            if (questionnaireTimerRoutine != null)
                StopCoroutine(questionnaireTimerRoutine);

            questionnaireTimerRoutine = StartCoroutine(QuestionnaireExpiryTimer());
        }
    }

    public void SetParticipantID(string id)
    {
        GameManager.participantID = id;

        new EventLogger.Event("Participant ID Changed", GameManager.participantID);
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

    private IEnumerator QuestionnaireExpiryTimer()
    {
        Debug.Log($"Session will expire after {playSessionIntervalMinutes} minutes");

        yield return new WaitForSecondsRealtime(playSessionIntervalMinutes * 60);

        playerPresentedWithQuestionnaire = true;

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(LoadedScenes.MidExperimentQuestionnaire);
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
