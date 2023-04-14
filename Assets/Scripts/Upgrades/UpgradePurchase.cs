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

    private LoadedScenes applicableScene = LoadedScenes.Sandbox;

    private string GetSaveKey()
    {
        return $"upgrade_{displayName}";
    }

    public void LoadState()
    {
        int purchasedStacks = SaveStateController.GetData<int>(GetSaveKey());

        // Prevent limit breaking by altering the save data
        if (purchasedStacks > maxStacks)
        {
            purchasedStacks = maxStacks;
            SaveStateController.SetData(GetSaveKey(), maxStacks);
        }

        for (int i = 0; i < purchasedStacks; i++)
            UpgradeLoader.AddPlayerUpgrade(this); ;
    }

    public bool HasCapacity()
    {
        return UpgradeLoader.HasSpaceForModifier(this, maxStacks);
    }

    public virtual void OnUnlocked()
    {
        UpgradeLoader.AddPlayerUpgrade(this);

        SaveStateController.SetData(GetSaveKey(), SaveStateController.GetData<int>(GetSaveKey()) + 1);
    }
    public virtual bool CanAfford()
    {
        return CurrencyHandler.CanAfford(cost) && HasCapacity();
    }

    public void ApplyUpgrade()
    {
        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() == applicableScene)
            OnUpgradeApplied();
    }

    protected abstract void OnUpgradeApplied();
}
