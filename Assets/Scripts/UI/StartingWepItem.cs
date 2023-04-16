using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class StartingWepItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image wepIcon;
    [SerializeField] private Image effectIcon;
    [SerializeField] private TextMeshProUGUI additiveDelayTxt, modifierList;

    [Header("Settings")]
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color chosenColour;

    private Button btnComponent;

    public UnityAction<int[]> onClicked;

    public void Setup(int[] dna, bool chosen = false)
    {
        btnComponent = GetComponent<Button>();
        ColorBlock colors = btnComponent.colors;
        colors.normalColor = chosen ? chosenColour : defaultColour;
        btnComponent.colors = colors;

        btnComponent.onClick.AddListener(() => onClicked.Invoke(dna));

        wepIcon.sprite = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna).icon;
        effectIcon.sprite = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna).icon;

        additiveDelayTxt.text = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModifierAdditiveDelay(dna).ToString();

        string modifierDescription = "";
        foreach (ProjectileModifier m in ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna))
        {
            modifierDescription += m.ToString() + "\n";
        }

        modifierList.text = modifierDescription;
    }
}
