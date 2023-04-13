using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillStation : Interactable
{
    [SerializeField] private int charges;
    [SerializeField] private bool infinite = false;

    public override string GetName()
    {
        if (!infinite)
            return $"Reload Station ({charges})";
        else
            return "Reload Station (∞)";
    }

    public override void OnInteracted()
    {
        charges--;

        ServiceLocator.instance.GetService<PlayerWeaponSystem>().ReloadEquippedWeapon();

        if (charges <= 0 && !infinite)
            Destroy(gameObject);
    }
}
