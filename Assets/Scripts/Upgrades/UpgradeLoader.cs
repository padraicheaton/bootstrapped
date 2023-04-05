using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeLoader
{
    private static UpgradePurchase[] availableUpgrades;

    private static GameObject playerRef;
    private static List<PlayerModifierUpgrade> playerModifiers = new List<PlayerModifierUpgrade>();

    public static UpgradePurchase[] GetAvailableUpgrades()
    {
        if (availableUpgrades == null || availableUpgrades.Length == 0)
            availableUpgrades = GetAllScriptableObjects<UpgradePurchase>();

        return availableUpgrades;
    }

    public static void AddPlayerUpgrade(PlayerModifierUpgrade upgrade)
    {
        playerModifiers.Add(upgrade);

        Debug.Log($"Added {upgrade.displayName}");
    }

    public static bool HasSpaceForModifier(PlayerModifierUpgrade upgrade, int maxAmount = 1)
    {
        int count = 0;

        foreach (PlayerModifierUpgrade pMU in playerModifiers)
            if (pMU.GetType() == upgrade.GetType())
                count++;

        return count < maxAmount;
    }

    public static void ApplyUpgradesToPlayer()
    {
        if (!playerRef)
            playerRef = ServiceLocator.instance.GetService<PlayerMovement>().gameObject;

        foreach (PlayerModifierUpgrade upgrade in playerModifiers)
            upgrade.ApplyModifierToPlayer(playerRef);
    }

    private static T[] GetAllScriptableObjects<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("");
    }
}
