using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : ModalWindow
{
    protected override void OnCloseMenuInput(bool performed)
    {
        if (performed)
        {
            SetVisibility(!isVisible, !isVisible);

            Time.timeScale = isVisible ? 0f : 1f;
        }
    }

    public void OnInfoBookBtnPressed()
    {

    }

    public void OnResumeBtnPressed()
    {
        OnCloseMenuInput(true);
    }

    public void OnSettingsBtnPressed()
    {
        ServiceLocator.instance.GetService<SettingsMenuController>().Show();
    }

    public void OnExitBtnPressed()
    {
        OnResumeBtnPressed();

        LoadedScenes backScene = LoadedScenes.MainMenu;

        switch (ServiceLocator.instance.GetService<SceneController>().GetActiveScene())
        {
            case LoadedScenes.Lab:
            default:
                backScene = LoadedScenes.MainMenu;
                break;
            case LoadedScenes.Sandbox:
                backScene = LoadedScenes.Lab;
                break;
        }

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(backScene);
    }
}
