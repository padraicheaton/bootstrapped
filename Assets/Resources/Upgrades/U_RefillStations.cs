using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Refill Stations", menuName = "Bootstrapped/Upgrades/Refill Stations")]
public class U_RefillStations : UpgradePurchase
{
    public override void ApplyUpgrade()
    {
        ServiceLocator.instance.GetService<LevelPopulator>().SpawnRefillStations();
    }
}
