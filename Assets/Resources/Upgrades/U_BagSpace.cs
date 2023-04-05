using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag Space", menuName = "Bootstrapped/Upgrades/Bag Space")]
public class U_BagSpace : PlayerModifierUpgrade
{
    public override void ApplyModifierToPlayer(GameObject player)
    {
        player.GetComponent<PlayerWeaponSystem>().IncreaseWeaponSpace(1);
    }
}
