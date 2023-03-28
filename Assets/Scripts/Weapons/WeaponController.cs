using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visuals;

    private bool IsActive;

    public void ShowWeapon(bool shown)
    {
        IsActive = shown;

        visuals.gameObject.SetActive(shown);
    }
}
