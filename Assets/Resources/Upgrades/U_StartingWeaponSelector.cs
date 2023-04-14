using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Starting Weapon Selector", menuName = "Bootstrapped/Upgrades/Starting Weapon Selector")]
public class U_StartingWeaponSelector : UpgradePurchase
{
    protected override LoadedScenes applicableScene { get { return LoadedScenes.Lab; } }

    protected override void OnUpgradeApplied()
    {
        ServiceLocator.instance.GetService<StartingWeaponSelectionShop>().Unlock();
    }
}
