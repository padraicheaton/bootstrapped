using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject exitPortal;
    [SerializeField] private VoiceLine VL_wavesCompleted;

    [Header("Settings")]
    [SerializeField] private int requiredWaveNum;

    private int wavesCompleted = 0;
    private bool identifiedWavesOver = false;

    private void Start()
    {
        exitPortal.SetActive(false);

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += OnWaveEnded;
        ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += OnWaveStarted;
    }

    public void OnWaveStarted()
    {
        string dialogueTxt = "Watch out! Enemies approaching!";

        ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(dialogueTxt);
    }

    public void OnWaveEnded()
    {
        wavesCompleted++;

        // if (wavesCompleted >= mutationChanceWaveThreshold)
        // {
        //     tutorialFoundryConfig.mutationChance = introductoryMutationChance;

        //     if (!identifiedMutationChange)
        //     {
        //         ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(VL_mutationBegun);

        //         identifiedMutationChange = true;
        //     }
        // }

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
