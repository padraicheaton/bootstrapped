using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : ModalWindow
{
    [Header("References")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeTextIndicator;


    protected override void Start()
    {
        base.Start();

        LoadSettings();

        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterVolumeValueChanged(); });
    }

    private void MasterVolumeValueChanged()
    {
        masterVolumeTextIndicator.text = masterVolumeSlider.value.ToString();
    }

    public void Show()
    {
        SetVisibility(true, true);

        LoadSettings();
    }

    protected override void Hide()
    {
        SetVisibility(false, true);

        SaveSettings();

        // Ensure music changes based on settings
        ServiceLocator.instance.GetService<SoundController>().SwitchBackgroundMusic(ServiceLocator.instance.GetService<SceneController>().GetActiveScene());
    }

    private void SaveSettings()
    {
        SaveStateController.SetData(SaveStateController.masterVolumeValueKey, masterVolumeSlider.value);
    }

    private void LoadSettings()
    {
        masterVolumeSlider.value = SaveStateController.GetData<float>(SaveStateController.masterVolumeValueKey);
        masterVolumeSlider.onValueChanged.Invoke(masterVolumeSlider.value);
    }
}
