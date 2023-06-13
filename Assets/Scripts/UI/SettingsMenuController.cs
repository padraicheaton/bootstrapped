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
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityTextIndicator;


    protected override void Start()
    {
        base.Start();

        LoadSettings();

        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterVolumeValueChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { SensitivityValueChanged(); });
    }

    private void MasterVolumeValueChanged()
    {
        masterVolumeTextIndicator.text = masterVolumeSlider.value.ToString();
    }

    private void SensitivityValueChanged()
    {
        sensitivityTextIndicator.text = sensitivitySlider.value.ToString("F1");
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
        SaveStateController.SetData(SaveStateController.sensitivityValueKey, sensitivitySlider.value);
    }

    private void LoadSettings()
    {
        if (SaveStateController.DatabaseContains(SaveStateController.masterVolumeValueKey))
            masterVolumeSlider.value = SaveStateController.GetData<float>(SaveStateController.masterVolumeValueKey);
        else
            masterVolumeSlider.value = 50f;
        masterVolumeSlider.onValueChanged.Invoke(masterVolumeSlider.value);

        if (SaveStateController.DatabaseContains(SaveStateController.sensitivityValueKey))
            sensitivitySlider.value = SaveStateController.GetData<float>(SaveStateController.sensitivityValueKey);
        else
            sensitivitySlider.value = 5f;
        sensitivitySlider.onValueChanged.Invoke(sensitivitySlider.value);
    }

    public void OnClearSaveDataBtnPressed()
    {
        SaveStateController.ClearSaveData();

        LoadSettings();
    }
}
