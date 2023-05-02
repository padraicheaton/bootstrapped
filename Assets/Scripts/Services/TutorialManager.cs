using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GenConfig tutorialFoundryConfig;
    [SerializeField] private GameObject exitPortal;
    [SerializeField] private VoiceLine VL_mutationBegun;
    [SerializeField] private VoiceLine VL_wavesCompleted;

    [Header("Settings")]
    [SerializeField] private int requiredWaveNum;
    [SerializeField] private int mutationChanceWaveThreshold;
    [SerializeField][Range(0f, 1f)] private float introductoryMutationChance;

    private int wavesCompleted = 0;
    private bool identifiedMutationChange = false;
    private bool identifiedWavesOver = false;

    private void Start()
    {
        tutorialFoundryConfig.mutationChance = 0f;

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += OnWaveEnded;
        ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += OnWaveStarted;
    }

    public void OnWaveStarted()
    {
        float percComplete = ((float)wavesCompleted / (float)requiredWaveNum) * 100f;
        int roundedPercentNum = Mathf.RoundToInt(percComplete);

        string dialogueTxt = $"Virus Injection {roundedPercentNum}% Complete. {requiredWaveNum - wavesCompleted} more injections should do it.";

        ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(dialogueTxt);
    }

    public void OnWaveEnded()
    {
        wavesCompleted++;

        if (wavesCompleted >= mutationChanceWaveThreshold)
        {
            tutorialFoundryConfig.mutationChance = introductoryMutationChance;

            if (!identifiedMutationChange)
            {
                ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(VL_mutationBegun);

                identifiedMutationChange = true;
            }
        }

        if (wavesCompleted >= requiredWaveNum)
        {
            exitPortal.SetActive(true);

            if (!identifiedWavesOver)
            {
                ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(VL_wavesCompleted);

                identifiedWavesOver = true;
            }
        }
    }
}
