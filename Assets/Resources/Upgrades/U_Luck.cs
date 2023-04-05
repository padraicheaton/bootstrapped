using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Luck", menuName = "Bootstrapped/Upgrades/Luck")]
public class U_Luck : PlayerModifierUpgrade
{
    public float additiveChance;

    public override void ApplyModifierToPlayer(GameObject player)
    {
        ServiceLocator.instance.GetService<LootController>().AddItemDropChance(additiveChance);
    }
}
