using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEditor.UI;

public class UpgradeShopItemUI : Button
{
    [Header("References")]
    public Image icon;
    public TextMeshProUGUI nameTxt, costTxt;

    UpgradePurchase upgradeData;

    public void Setup(UpgradePurchase upgradeData)
    {
        this.upgradeData = upgradeData;

        icon.sprite = upgradeData.icon;
        nameTxt.text = upgradeData.displayName;
        costTxt.text = upgradeData.cost.ToString();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(UpgradeShopItemUI))]
public class UpgradeShopItemUIEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        UpgradeShopItemUI item = (UpgradeShopItemUI)target;

        item.icon = EditorGUILayout.ObjectField("Icon", item.icon, typeof(Image), true) as Image;
        item.nameTxt = EditorGUILayout.ObjectField("Name Txt", item.nameTxt, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
        item.costTxt = EditorGUILayout.ObjectField("Cost Txt", item.costTxt, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;

        EditorUtility.SetDirty(item.gameObject);

        base.OnInspectorGUI();
    }
}

#endif