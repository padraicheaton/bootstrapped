using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image remainingAmmoChargeImg;
    [SerializeField] private CanvasGroup remainingAmmoCG;
    [SerializeField] private TextMeshProUGUI sparePartCountTxt;
    [SerializeField] private CanvasGroup crosshairCG;

    [Header("Settings")]
    [SerializeField] private float healthBarLerpSpeed;
    [SerializeField] private float ammoAlphaSpeed;

    public bool shouldShowCrosshair = true;

    private void Update()
    {
        healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetHealthPercentage(), Time.deltaTime * healthBarLerpSpeed);

        float ammoCharge = ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetEquippedWeaponAmmoPercentage();

        if (ammoCharge == -1)
            remainingAmmoCG.alpha = Mathf.Lerp(remainingAmmoCG.alpha, 0f, Time.deltaTime * ammoAlphaSpeed);
        else
        {
            remainingAmmoChargeImg.color = ServiceLocator.instance.GetService<PlayerWeaponSystem>().GetEquippedWeaponReloadingState() ? Color.red : Color.cyan;
            remainingAmmoChargeImg.fillAmount = Mathf.Lerp(remainingAmmoChargeImg.fillAmount, ammoCharge, Time.deltaTime * ammoAlphaSpeed);
            remainingAmmoCG.alpha = Mathf.Lerp(remainingAmmoCG.alpha, 1f, Time.deltaTime * ammoAlphaSpeed);
        }

        crosshairCG.alpha = Mathf.Lerp(crosshairCG.alpha, shouldShowCrosshair ? 1f : 0f, Time.deltaTime * ammoAlphaSpeed);

        sparePartCountTxt.text = CurrencyHandler.GetSparePartCount().ToString();
    }
}
