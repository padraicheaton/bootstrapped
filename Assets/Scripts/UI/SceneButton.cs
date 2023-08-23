using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LoadedScenes sceneToLoad;

    private void Start()
    {
        Button btnComponent = GetComponent<Button>();

        btnComponent.onClick.AddListener(() =>
        {
            ServiceLocator.instance.GetService<SceneController>().SwitchSceneTo(sceneToLoad);
        });
    }
}
