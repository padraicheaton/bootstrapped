using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Transform weaponFirePoint;

    [Header("Settings")]
    [SerializeField] private GameObject[] startingWeapons;

    private int weaponSlotCount = 4;
    private List<WeaponController> weaponSlots = new List<WeaponController>();
    private int activeWeaponIndex;

    private void Start()
    {
        activeWeaponIndex = -1;

        foreach (GameObject startingWeapon in startingWeapons)
        {
            GameObject wep = Instantiate(startingWeapon, Vector3.zero, Quaternion.identity);

            AddWeapon(wep);
        }

        SwitchToWeaponIndex(0);

        SetupControls();
    }

    private void SetupControls()
    {
        ServiceLocator.instance.GetService<InputManager>().OnFireButton += OnFireInput;
    }

    private void OnFireInput(bool performed)
    {
        if (weaponSlots[activeWeaponIndex])
        {
            weaponSlots[activeWeaponIndex].OnFireInput(performed);
        }
    }

    private void SwitchToWeaponIndex(int newWeaponIndex)
    {
        if (weaponSlots[newWeaponIndex] == null) return;

        // Handle unequipping current weapon
        if (activeWeaponIndex != -1 && weaponSlots[activeWeaponIndex] != null)
            weaponSlots[activeWeaponIndex].ShowWeapon(false);

        // Show weapon
        weaponSlots[newWeaponIndex].ShowWeapon(true);
        activeWeaponIndex = newWeaponIndex;
    }

    public void AddWeapon(GameObject weapon)
    {
        if (!HasCapacity()) return;

        // Move weapon to container
        weapon.transform.SetParent(weaponContainer);
        weapon.transform.localPosition = weapon.transform.localEulerAngles = Vector3.zero;

        // Cache reference to it
        WeaponController weaponController = weapon.GetComponent<WeaponController>();

        //! DEBUG, WILL BE CALLED ON GENERATION
        weaponController.Construct(new int[] { });

        // Call on picked up
        weaponController.OnPickedUp(weaponFirePoint);

        // Fill first empty slot in array
        weaponSlots.Add(weaponController);

        // Deactivate the weapon by default
        weaponController.ShowWeapon(false);
    }

    private bool HasCapacity()
    {
        return weaponSlots.Count < weaponSlotCount;
    }
}
