using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradePurchase : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    [TextArea] public string description;
    public int cost;
    public int maxStacks;

    public bool HasCapacity()
    {
        return UpgradeLoader.HasSpaceForModifier(this, maxStacks);
    }

    public virtual void OnUnlocked()
    {
        UpgradeLoader.AddPlayerUpgrade(this);
    }
    public virtual bool CanAfford()
    {
        return CurrencyHandler.CanAfford(cost) && HasCapacity();
    }

    public abstract void ApplyUpgrade();
}
