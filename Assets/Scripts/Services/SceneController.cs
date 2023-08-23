using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum LoadedScenes
{
    MainMenu,
    Lab,
    Sandbox,
    Tutorial,
    Tutorial_Foundry,
    Sandbox_Random,
    MidExperimentQuestionnaire
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreen;

    public UnityAction onSceneChanged;

    private List<LoadedScenes> scenesWithMouseControl = new List<LoadedScenes>() { LoadedScenes.MainMenu, LoadedScenes.MidExperimentQuestionnaire };

    private void Start()
    {
        loadingScreen.alpha = 0f;

        ServiceLocator.instance.GetService<SoundController>().SwitchBackgroundMusic(GetActiveScene());

        if (ServiceLocator.instance.GetService<SceneController>() != this)
        {
            // Already exists
            Destroy(gameObject);
        }
        else if (transform.parent != null)
        {
            transform.SetParent(null);

            DontDestroyOnLoad(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    public void SwitchSceneTo(LoadedScenes scene)
    {
        StartCoroutine(BloatLoadScene(scene));
    }

    public LoadedScenes GetActiveScene()
    {
        return (LoadedScenes)SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator BloatLoadScene(LoadedScenes scene)
    {
        while (loadingScreen.alpha < 1f)
        {
            loadingScreen.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene((int)scene);

        ServiceLocator.instance.GetService<SoundController>().SwitchBackgroundMusic(scene);

        yield return new WaitForSeconds(0.5f);

        if (onSceneChanged != null)
            onSceneChanged.Invoke();

        while (loadingScreen.alpha > 0f)
        {
            loadingScreen.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        if (scenesWithMouseControl.Contains(scene))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
