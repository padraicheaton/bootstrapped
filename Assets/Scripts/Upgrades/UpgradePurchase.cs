using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradePurchase : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    [TextArea] public string description;
    public int cost;

    public abstract void OnUnlocked();
    public virtual bool CanAfford()
    {
        return CurrencyHandler.CanAfford(cost);
    }
}
