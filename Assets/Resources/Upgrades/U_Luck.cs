using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Luck", menuName = "Bootstrapped/Upgrades/Luck")]
public class U_Luck : UpgradePurchase
{
    public float additiveChance;

    public override void ApplyUpgrade()
    {
        ServiceLocator.instance.GetService<LootController>().AddItemDropChance(additiveChance);
    }
}
