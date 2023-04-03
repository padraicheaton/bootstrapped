using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI remainingAmmoTxt;
    [SerializeField] private CanvasGroup remainingAmmoCG;

    [Header("Settings")]
    [SerializeField] private float healthBarLerpSpeed;
    [SerializeField] private float ammoAlphaSpeed;

    private void Update()
    {
        healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealthPercentage(), Time.deltaTime * healthBarLerpSpeed);

        int remainingAmmo = ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetEquippedWeaponAmmo();

        if (remainingAmmo == -1)
            remainingAmmoCG.alpha = Mathf.Lerp(remainingAmmoCG.alpha, 0f, Time.deltaTime * ammoAlphaSpeed);
        else
        {
            remainingAmmoTxt.text = remainingAmmo.ToString();
            remainingAmmoCG.alpha = Mathf.Lerp(remainingAmmoCG.alpha, 1f, Time.deltaTime * ammoAlphaSpeed);
        }
    }
}
