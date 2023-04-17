using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartInteractable : Interactable
{
    public override string GetName()
    {
        return "Inject Virus";
    }

    public override void OnInteracted()
    {
        ServiceLocator.instance.GetService<Spawner>().BeginSwarm();
    }
}
