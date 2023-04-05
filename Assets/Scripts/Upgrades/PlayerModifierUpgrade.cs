using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerModifierUpgrade : UpgradePurchase
{
    public int maxStacks;

    public override void OnUnlocked()
    {
        UpgradeLoader.AddPlayerUpgrade(this);
    }

    public bool HasCapacity()
    {
        return UpgradeLoader.HasSpaceForModifier(this, maxStacks);
    }

    public override bool CanAfford()
    {
        return base.CanAfford() && HasCapacity();
    }

    public abstract void ApplyModifierToPlayer(GameObject player);
}
