using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingWeaponSelectionShop : Interactable
{
    [Header("References")]
    [SerializeField] private GameObject visualsParent;

    private readonly int[] defaultStartingWeapon = new int[] { 1, 0, 0, 0, 0 };

    private int[] chosenStartingWeapon;

    private Collider coll;

    private void Start()
    {
        if (!SaveStateController.DatabaseContains(SaveStateController.startingWeaponGenotypeKey))
            SaveStateController.SetData(SaveStateController.startingWeaponGenotypeKey, defaultStartingWeapon);

        chosenStartingWeapon = SaveStateController.GetData<int[]>(SaveStateController.startingWeaponGenotypeKey);

        coll = GetComponent<Collider>();

        SetState(false);
    }

    public override string GetName()
    {
        return "Open Weapon Selector";
    }

    public void Unlock()
    {
        SetState(true);
    }

    private void SetState(bool unlocked)
    {
        IsInteractable = unlocked;
        visualsParent.SetActive(unlocked);
        coll.enabled = unlocked;
    }

    public override void OnInteracted()
    {
        Debug.Log("Open Up Weapon Selector");
    }
}
