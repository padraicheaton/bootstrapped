using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadedScenes
{
    Lab,
    Sandbox
}

public class SceneController : MonoBehaviour
{
    public void SwitchSceneTo(LoadedScenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public LoadedScenes GetActiveScene()
    {
        return (LoadedScenes)SceneManager.GetActiveScene().buildIndex;
    }
}
