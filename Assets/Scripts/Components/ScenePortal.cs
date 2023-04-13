using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : Interactable
{
    public LoadedScenes destination;

    public override string GetName()
    {
        return $"To: The {destination.ToString()}";
    }

    public override void OnInteracted()
    {
        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == LoadedScenes.Tutorial)
            SaveStateController.SetData(SaveStateController.tutorialCompleteSaveKey, true);

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(destination);
    }
}
