using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartingWeaponSelectionShop : Interactable
{
    [Header("References")]
    [SerializeField] private GameObject visualsParent;
    [SerializeField] private RectTransform shopInventoryGridParent;
    [SerializeField] private GameObject startingWepItemPrefab;
    [SerializeField] private ModalWindow shopWindow;
    [SerializeField] private TextMeshProUGUI costTxt;
    [SerializeField] private Button buyBtn;

    private readonly int cost = 1;

    private int[] chosenStartingWeapon;
    private int[] selectedShopItem;
    private List<int[]> shopInventory;

    private Collider coll;

    private bool setup = false;

    private void Start()
    {
        coll = GetComponent<Collider>();

        costTxt.text = cost.ToString();

        buyBtn.interactable = false;
        buyBtn.onClick.AddListener(OnBuyBtnPressed);

        SetState(false);
    }

    private void PopulateInventory(int amount)
    {
        chosenStartingWeapon = WeaponDataCollector.GetStartingWeaponGenotype();

        if (chosenStartingWeapon == null)
        {
            chosenStartingWeapon = WeaponDataCollector.defaultStartingWeapon;
            WeaponDataCollector.SetStartingWeapon(chosenStartingWeapon);
        }

        if (shopInventory == null)
            shopInventory = new List<int[]>();
        else
            shopInventory.Clear();

        shopInventory.Add(chosenStartingWeapon);

        for (int i = 0; i < amount - 1; i++)
        {
            shopInventory.Add(EvolutionAlgorithms.Randomised());
        }
    }

    private void PopulateUI()
    {
        for (int i = shopInventoryGridParent.childCount - 1; i >= 0; i--)
        {
            Destroy(shopInventoryGridParent.GetChild(i).gameObject);
        }

        foreach (int[] dna in shopInventory)
        {
            GameObject item = Instantiate(startingWepItemPrefab, shopInventoryGridParent);

            StartingWepItem itemScript = item.GetComponent<StartingWepItem>();
            itemScript.Setup(dna, dna == chosenStartingWeapon);
            itemScript.onClicked += OnWeaponItemSelected;
        }
    }

    private void OnWeaponItemSelected(int[] selectedItem)
    {
        buyBtn.interactable = CurrencyHandler.CanAfford(cost);

        selectedShopItem = selectedItem;
    }

    public void OnBuyBtnPressed()
    {
        chosenStartingWeapon = selectedShopItem;

        WeaponDataCollector.SetStartingWeapon(selectedShopItem);

        CurrencyHandler.DecreaseSparePartCount(cost);

        selectedShopItem = null;

        buyBtn.interactable = false;

        PopulateUI();
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
        if (!setup)
        {
            PopulateInventory(10);

            PopulateUI();

            setup = true;
        }

        shopWindow.SetVisibility(true, true);
    }
}
