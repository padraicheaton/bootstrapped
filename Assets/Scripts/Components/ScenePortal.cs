using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : Interactable
{
    [Header("Settings")]
    public LoadedScenes destination;
    public string overrideDestinationName;

    [Header("Time Sensitive Settings")]
    public LoadedScenes timeSensitiveDestination;

    public override string GetName()
    {
        if (overrideDestinationName == "" || overrideDestinationName == null)
            return $"To: The {destination.ToString()}";
        else
            return $"To: {overrideDestinationName}";
    }

    public override void OnInteracted()
    {
        if (destination == LoadedScenes.Lab)
            SaveStateController.SetData(SaveStateController.tutorialCompleteSaveKey, true);

        if (destination != timeSensitiveDestination && GameManager.playerPresentedWithQuestionnaire)
        {
            destination = timeSensitiveDestination;
        }

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(destination);
    }
}
