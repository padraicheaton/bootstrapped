using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Transform aimHoldPoint;
    [SerializeField] private Transform weaponFirePoint;

    [Header("Settings")]
    [SerializeField] private bool useStartingWeapon = true;

    public int weaponSlotCount { get; private set; } = 2;
    private List<WeaponController> weaponSlots = new List<WeaponController>();
    private int activeWeaponIndex;

    private HealthComponent playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<HealthComponent>();

        activeWeaponIndex = -1;

        if (useStartingWeapon)
        {
            StartCoroutine(EquipStartingWeaponAfterDelay());
        }

        SetupControls();
    }

    private IEnumerator EquipStartingWeaponAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        int[] startingWeapon = WeaponDataCollector.GetStartingWeaponGenotype();

        GameObject wep = ServiceLocator.instance.GetService<WeaponGenerator>().GenerateWeapon(startingWeapon, Vector3.zero);

        AddWeapon(wep);

        SwitchToWeaponIndex(0);
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
        ServiceLocator.instance.GetService<InputManager>().OnAimButton += OnAimInput;
    }

    private void OnFireInput(bool performed)
    {
        if (ServiceLocator.instance.GetService<PlayerMovement>().CanMove && weaponSlots.Count > 0 && GetActiveWeapon())
        {
            GetActiveWeapon().OnFireInput(performed);
        }
    }

    private void OnAimInput(bool performed)
    {
        if (GetActiveWeapon())
        {
            GetActiveWeapon().transform.position = performed ? aimHoldPoint.position : weaponContainer.position;
            GetActiveWeapon().transform.rotation = performed ? aimHoldPoint.rotation : weaponContainer.rotation;

            ServiceLocator.instance.GetService<PlayerCamera>().SetFOV(performed);
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

    public int[] GetActiveWeaponGenotype()
    {
        return GetActiveWeapon().dna;
    }

    public void ReloadEquippedWeapon()
    {
        if (GetActiveWeapon())
            GetActiveWeapon().ReloadWeapon();
    }

    public float GetHealthPercentage()
    {
        return playerHealth.GetHealthPercentage();
    }

    public HealthComponent GetHealth()
    {
        return playerHealth;
    }

    public int GetEquippedWeaponAmmo()
    {
        if (GetActiveWeapon() == null)
            return -1;
        else
            return GetActiveWeapon().remainingAmmo;
    }

    public void IncreaseWeaponSpace(int amt)
    {
        weaponSlotCount += amt;
    }
}
