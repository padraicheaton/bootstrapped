using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerModifierUpgrade : UpgradePurchase
{
    public override void ApplyUpgrade()
    {
        ApplyModifierToPlayer(ServiceLocator.instance.GetService<PlayerMovement>().gameObject);
    }

    public abstract void ApplyModifierToPlayer(GameObject player);
}
