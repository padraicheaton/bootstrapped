using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyHandler
{
    private static string sparePartsSaveKey = "spareParts";

    private static int spareParts = 100;

    public static void Setup()
    {
        spareParts = SaveStateController.GetData<int>(sparePartsSaveKey);
    }

    public static void IncreaseSparePartCount(int amt)
    {
        spareParts += amt;
        UpdateSaveData();
    }

    public static void DecreaseSparePartCount(int amt)
    {
        spareParts -= amt;
        UpdateSaveData();
    }

    public static int GetSparePartCount()
    {
        return spareParts;
    }

    public static bool CanAfford(int cost)
    {
        return cost <= spareParts;
    }

    private static void UpdateSaveData()
    {
        SaveStateController.SetData(sparePartsSaveKey, spareParts);
    }
}
