using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerModifierUpgrade : UpgradePurchase
{
    public int maxStacks;
    public int currentStacks;

    public override void OnUnlocked()
    {
        UpgradeLoader.AddPlayerUpgrade(this);
    }

    public bool HasUnlocked()
    {
        return UpgradeLoader.PlayerModifiersContains(this);
    }
    public override bool CanAfford()
    {
        return base.CanAfford() && !HasUnlocked();
    }

    public abstract void ApplyModifierToPlayer(GameObject player);
}
