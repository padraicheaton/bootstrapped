using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ModalWindow creditsWindow;

    private void Start()
    {
        CanvasGroup[] cgs = GetComponentsInChildren<CanvasGroup>();

        for (int i = 0; i < cgs.Length; i++)
            StartCoroutine(FadeInUI(cgs[i], i / 4f));
    }

    private IEnumerator FadeInUI(CanvasGroup cg, float delay)
    {
        cg.alpha = 0f;

        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(delay);

        while (cg.alpha < 1f)
        {
            yield return null;
            cg.alpha += Time.deltaTime;
        }
    }

    public void PlayBtnPressed()
    {
        // ! Implement a switch here so that it only loads the tutorial when its incomplete
        bool hasCompletedTutorial = SaveStateController.GetData<bool>(SaveStateController.tutorialCompleteSaveKey);

        ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(hasCompletedTutorial ? LoadedScenes.Lab : LoadedScenes.Tutorial);
    }

    public void SettingsBtnPressed()
    {
        ServiceLocator.instance.GetService<SettingsMenuController>().Show();
    }

    public void CreditsBtnPressed()
    {
        creditsWindow.SetVisibility(true);
    }

    public void ExitBtnPressed()
    {
        Application.Quit();
    }
}
