using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillStation : Interactable
{
    [SerializeField] private int charges;

    public override string GetName()
    {
        return $"Ammo Refill Station ({charges})";
    }

    public override void OnInteracted()
    {
        charges--;

        ServiceLocator.instance.GetService<PlayerWeaponSystem>().ReloadEquippedWeapon();

        if (charges <= 0)
            Destroy(gameObject);
    }
}
