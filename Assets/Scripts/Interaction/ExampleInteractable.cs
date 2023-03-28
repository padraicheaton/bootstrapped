using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInteractable : Interactable
{
    public override void OnInteracted()
    {
        Debug.Log("Interacted");
    }
}
