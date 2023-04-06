using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : Interactable
{
    [Header("References")]
    [SerializeField] private ModalWindow shopModal;
    [SerializeField] private Transform upgradeListContainer;
    [SerializeField] private Transform stackPipContainer;
    [SerializeField] private GameObject upgradeShopItemPrefab;

    [Header("Item Details Panel Refs")]
    [SerializeField] private CanvasGroup itemDetailsCG;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI nameTxt, descTxt, costTxt;
    [SerializeField] private Button purchaseBtn;

    private List<UpgradePurchase> upgrades;

    private UpgradePurchase focusedUpgrade;

    private void Start()
    {
        upgrades = new List<UpgradePurchase>();

        upgrades.AddRange(UpgradeLoader.GetAvailableUpgrades());

        foreach (UpgradePurchase upgrade in upgrades)
        {
            GameObject item = Instantiate(upgradeShopItemPrefab, upgradeListContainer);

            UpgradeShopItemUI controller = item.GetComponent<UpgradeShopItemUI>();

            controller.Setup(upgrade);

            controller.onClick.AddListener(() => ShowUpgradeDetails(upgrade));
        }

        purchaseBtn.onClick.AddListener(() => OnPurchaseButtonClicked());

        ShowUpgradeDetails(null);
    }

    private void ShowUpgradeDetails(UpgradePurchase upgrade)
    {
        itemDetailsCG.alpha = upgrade != null ? 1f : 0f;
        itemDetailsCG.interactable = itemDetailsCG.blocksRaycasts = upgrade != null;

        if (upgrade != null)
        {
            focusedUpgrade = upgrade;

            itemIcon.sprite = upgrade.icon;
            nameTxt.text = upgrade.displayName;
            descTxt.text = upgrade.description;
            costTxt.text = upgrade.cost.ToString();

            purchaseBtn.interactable = upgrade.CanAfford();

            // Show Stack Pips
            if (stackPipContainer.childCount != upgrade.maxStacks)
            {
                for (int i = stackPipContainer.childCount - 1; i > 0; i--)
                {
                    Destroy(stackPipContainer.GetChild(i).gameObject);
                }

                for (int i = 1; i < upgrade.maxStacks; i++)
                {
                    Instantiate(stackPipContainer.GetChild(0).gameObject, stackPipContainer);
                }
            }

            for (int i = 0; i < stackPipContainer.childCount; i++)
            {
                Image img = stackPipContainer.GetChild(i).GetComponent<Image>();

                img.color = UpgradeLoader.GetAmountOfUpgrades(upgrade) > i ? Color.yellow : Color.grey;
            }
        }
    }

    private void OnPurchaseButtonClicked()
    {
        if (focusedUpgrade)
        {
            CurrencyHandler.DecreaseSparePartCount(focusedUpgrade.cost);

            focusedUpgrade.OnUnlocked();

            ShowUpgradeDetails(focusedUpgrade);
        }
    }

    public override string GetName()
    {
        return "Open";
    }
    public override void OnInteracted()
    {
        shopModal.SetVisibility(true, true);
    }
}
