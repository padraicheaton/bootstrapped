using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePortal : Interactable
{
    public LoadedScenes destination;

    public override string GetName()
    {
        return $"To: {destination.ToString()}";
    }

    public override void OnInteracted()
    {
        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(destination);
    }
}
