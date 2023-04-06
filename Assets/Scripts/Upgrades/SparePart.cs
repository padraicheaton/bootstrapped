using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparePart : PhysicsPickup
{
    [Header("Settings")]
    [SerializeField] private int value;


    protected override void Collect()
    {
        CurrencyHandler.IncreaseSparePartCount(value);
    }
}
