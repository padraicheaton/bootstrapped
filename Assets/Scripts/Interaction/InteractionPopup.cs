using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionPopup : MonoBehaviour
{

    [Header("General Interactable")]
    [SerializeField] private CanvasGroup generalCG;
    [SerializeField] private TextMeshProUGUI displayNameTxt;

    [Header("Weapon Interactable")]
    [SerializeField] private CanvasGroup weaponCG;
    [SerializeField] private TextMeshProUGUI weaponNameTxt;
    [SerializeField] private TextMeshProUGUI weaponModifiersTxt;
    [SerializeField] private TextMeshProUGUI weaponAdditiveDelayTxt;
    [SerializeField] private TextMeshProUGUI weaponAmmoTxt;
    [SerializeField] private Image effectIcon;

    private Transform cameraTransform;
    private Vector3 rayCastPos;

    private bool isShown;
    private CanvasGroup baseCG;

    private void Start()
    {
        baseCG = GetComponent<CanvasGroup>();
        cameraTransform = ServiceLocator.instance.GetService<PlayerCamera>().transform;

        baseCG.alpha = 0f;
    }

    public void SetVisibleState(bool shown)
    {
        isShown = shown;

        if (!shown)
            rayCastPos += Vector3.down * 5f;
    }

    public void SetData(Vector3 pos, Interactable interactable)
    {
        rayCastPos = pos;

        if (interactable is WeaponController)
        {
            generalCG.alpha = 0f;
            weaponCG.alpha = 1f;

            int[] dna = ((WeaponController)interactable).dna;

            weaponNameTxt.text = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetWeaponObject(dna).displayName;
            effectIcon.sprite = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetEffectObject(dna).icon;
            weaponAdditiveDelayTxt.text = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetModifierAdditiveDelay(dna) + "s";
            weaponAmmoTxt.text = ((WeaponController)interactable).remainingAmmo.ToString();

            string modifierDescription = "";

            ProjectileModifier[] modifiers = ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna);

            // Number the list
            for (int i = 0; i < modifiers.Length; i++)
            {
                modifierDescription += $"{i + 1}. {modifiers[i].ToString()}";
            }

            // foreach (ProjectileModifier m in ServiceLocator.instance.GetService<WeaponComponentProvider>().GetProjectileModifiers(dna))
            // {
            //     modifierDescription += m.ToString() + "\n";
            // }

            weaponModifiersTxt.text = modifierDescription;
        }
        else
        {
            generalCG.alpha = 1f;
            weaponCG.alpha = 0f;

            displayNameTxt.text = "(E)\n" + interactable.GetName();
        }
    }

    private void Update()
    {
        baseCG.alpha = Mathf.Lerp(baseCG.alpha, isShown ? 1f : 0f, Time.deltaTime * 10f);

        if (isShown)
        {
            transform.position = Vector3.Lerp(transform.position, rayCastPos, Time.deltaTime * 15f);

            Vector3 targetDirection = transform.position - cameraTransform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime, 0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
