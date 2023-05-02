using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : Interactable
{
    [Header("Settings")]
    public LoadedScenes destination;
    public string overrideDestinationName;

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

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(destination);
    }
}
