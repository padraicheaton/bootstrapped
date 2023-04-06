using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PhysicsPickup
{
    [SerializeField] private float healthValue;

    protected override void Collect()
    {
        ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealth().Heal(healthValue);
    }
}
