using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Transform weaponFirePoint;

    [Header("Settings")]
    [SerializeField] private int[] startingWeapon;

    public int weaponSlotCount { get; private set; } = 3;
    private List<WeaponController> weaponSlots = new List<WeaponController>();
    private int activeWeaponIndex;

    private void Start()
    {
        activeWeaponIndex = -1;

        GameObject wep = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(startingWeapon, Vector3.zero);

        AddWeapon(wep);

        SwitchToWeaponIndex(0);

        SetupControls();
    }

    private void Update()
    {
        float scrollValue = ServiceLocator.instance.GetService<InputManager>().GetScrollInput();

        if (scrollValue != 0)
        {
            SwitchWeapons(Mathf.RoundToInt(Mathf.Sign(scrollValue)));
        }
    }

    private void SetupControls()
    {
        ServiceLocator.instance.GetService<InputManager>().OnFireButton += OnFireInput;
        ServiceLocator.instance.GetService<InputManager>().OnDropWeapon += OnDropInput;
    }

    private void OnFireInput(bool performed)
    {
        if (weaponSlots.Count > 0 && GetActiveWeapon())
        {
            GetActiveWeapon().OnFireInput(performed);
        }
    }

    private void OnDropInput(bool performed)
    {
        if (performed)
            DropActiveWeapon();
    }

    private void SwitchWeapons(int direction)
    {
        int newIndex = activeWeaponIndex + direction;

        if (newIndex > weaponSlots.Count - 1) newIndex = 0;
        if (newIndex < 0) newIndex = weaponSlots.Count - 1;

        SwitchToWeaponIndex(newIndex);
    }

    private void SwitchToWeaponIndex(int newWeaponIndex)
    {
        if (weaponSlots.Count == 0 || weaponSlots[newWeaponIndex] == null) return;

        // Handle unequipping current weapon
        if (GetActiveWeapon() != null)
            GetActiveWeapon().ShowWeapon(false);

        // Show weapon
        weaponSlots[newWeaponIndex].ShowWeapon(true);
        activeWeaponIndex = newWeaponIndex;

        ServiceLocator.instance.GetService<WeaponSwapHUDController>().RepopulateHUD(GetEquippedWeapons(), activeWeaponIndex);
    }

    private void DropActiveWeapon()
    {
        if (!GetActiveWeapon()) return;

        GetActiveWeapon().transform.SetParent(null);

        GetActiveWeapon().OnDropped();
        weaponSlots.RemoveAt(activeWeaponIndex);

        if (weaponSlots.Count > 0 && weaponSlots[0])
            SwitchToWeaponIndex(0);

        ServiceLocator.instance.GetService<WeaponSwapHUDController>().RepopulateHUD(GetEquippedWeapons(), activeWeaponIndex);
    }

    public void AddWeapon(GameObject weapon)
    {
        if (!HasCapacity()) return;

        // Move weapon to container
        weapon.transform.SetParent(weaponContainer);
        weapon.transform.localPosition = weapon.transform.localEulerAngles = Vector3.zero;

        // Cache reference to it
        WeaponController weaponController = weapon.GetComponent<WeaponController>();

        // Call on picked up
        weaponController.OnPickedUp(weaponFirePoint);

        // Fill first empty slot in array
        weaponSlots.Add(weaponController);

        // Deactivate the weapon by default
        weaponController.ShowWeapon(false);

        // Switch to it if first weapon picked up
        if (weaponSlots.Count == 1)
            SwitchToWeaponIndex(0);

        // Update UI
        ServiceLocator.instance.GetService<WeaponSwapHUDController>().RepopulateHUD(GetEquippedWeapons(), activeWeaponIndex);
    }

    private WeaponController GetActiveWeapon()
    {
        if (weaponSlots.Count > activeWeaponIndex && activeWeaponIndex >= 0)
        {
            return weaponSlots[activeWeaponIndex];
        }
        else
            return null;
    }

    private bool HasCapacity()
    {
        return weaponSlots.Count < weaponSlotCount;
    }

    public List<WeaponController> GetEquippedWeapons()
    {
        return weaponSlots;
    }
}
