using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeLoader
{
    private static UpgradePurchase[] availableUpgrades;

    private static GameObject playerRef;
    private static List<UpgradePurchase> loadedUpgrades = new List<UpgradePurchase>();

    public static UpgradePurchase[] GetAvailableUpgrades()
    {
        if (availableUpgrades == null || availableUpgrades.Length == 0)
        {
            availableUpgrades = GetAllScriptableObjects<UpgradePurchase>();

            foreach (UpgradePurchase upgrade in availableUpgrades)
            {
                upgrade.LoadState();
            }
        }

        return availableUpgrades;
    }

    public static void AddPlayerUpgrade(UpgradePurchase upgrade)
    {
        loadedUpgrades.Add(upgrade);
    }

    public static bool HasSpaceForModifier(UpgradePurchase upgrade, int maxAmount = 1)
    {
        return GetAmountOfUpgrades(upgrade) < maxAmount;
    }

    public static int GetAmountOfUpgrades(UpgradePurchase upgrade)
    {
        int count = 0;

        foreach (UpgradePurchase pMU in loadedUpgrades)
            if (pMU.GetType() == upgrade.GetType())
                count++;

        return count;
    }

    public static void ApplyUpgradesToPlayer()
    {
        foreach (UpgradePurchase upgrade in loadedUpgrades)
            upgrade.ApplyUpgrade();
    }

    private static T[] GetAllScriptableObjects<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("");
    }
}
