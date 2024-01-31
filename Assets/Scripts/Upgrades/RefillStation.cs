using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillStation : Interactable
{
    [SerializeField] private int charges;
    [SerializeField] private bool infinite = false;

    private void Start()
    {
        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.Tutorial)
            ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += () =>
            {
                charges = 3;
            };
    }

    public override string GetName()
    {
        if (infinite)
            return "Reload";
        else if (charges > 0)
            return $"Reload ({charges})";
        else
            return "Reload (EMPTY)";
    }

    public override void OnInteracted()
    {
        if (charges > 0)
        {
            ServiceLocator.instance.GetService<PlayerWeaponSystem>().ReloadEquippedWeapon();

            if (!infinite)
                charges--;
        }
    }
}
