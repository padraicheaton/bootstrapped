using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadedScenes
{
    MainMenu,
    Lab,
    Sandbox,
    Tutorial
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreen;

    private void Start()
    {
        loadingScreen.alpha = 0f;

        ServiceLocator.instance.GetService<SoundController>().SwitchBackgroundMusic(GetActiveScene());

        if (ServiceLocator.instance.GetService<SceneController>() != this)
        {
            // Already exists
            Destroy(gameObject);
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

        while (loadingScreen.alpha > 0f)
        {
            loadingScreen.alpha -= Time.deltaTime * 2f;
            yield return null;
        }
    }
}
