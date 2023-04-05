using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyHandler
{
    private static int spareParts = 0;

    public static void IncreaseSparePartCount(int amt)
    {
        spareParts += amt;
    }

    public static void DecreaseSparePartCount(int amt)
    {
        spareParts -= amt;
    }

    public static int GetSparePartCount()
    {
        return spareParts;
    }
}
