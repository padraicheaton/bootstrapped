using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBasedInteractable : Interactable
{
    private Vector3 startingPos, hiddenPos;
    protected virtual void Start()
    {
        startingPos = transform.position;
        hiddenPos = startingPos + Vector3.down * 10f;

        ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += () =>
        {
            IsInteractable = false;
        };

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += () =>
        {
            IsInteractable = true;
        };
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, IsInteractable ? startingPos : hiddenPos, Time.deltaTime);
    }

    public override void OnInteracted()
    {

    }
}
