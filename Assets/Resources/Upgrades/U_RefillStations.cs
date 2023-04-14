using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Refill Stations", menuName = "Bootstrapped/Upgrades/Refill Stations")]
public class U_RefillStations : UpgradePurchase
{
    protected override void OnUpgradeApplied()
    {
        ServiceLocator.instance.GetService<LevelPopulator>().SpawnRefillStations();
    }
}
