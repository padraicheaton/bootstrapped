using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerModifierUpgrade : UpgradePurchase
{
    protected override void OnUpgradeApplied()
    {
        ApplyModifierToPlayer(ServiceLocator.instance.GetService<PlayerMovement>().gameObject);
    }

    public abstract void ApplyModifierToPlayer(GameObject player);
}
