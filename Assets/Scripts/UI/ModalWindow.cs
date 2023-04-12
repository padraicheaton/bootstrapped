using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    private CanvasGroup cg;
    private float destAlpha;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        ServiceLocator.instance.GetService<InputManager>().OnCloseMenu += OnCloseMenuInput;

        foreach (Button btn in GetComponentsInChildren<Button>())
        {
            if (btn.name.Contains("Close"))
            {
                btn.onClick.AddListener(() => SetVisibility(false));
                break;
            }
        }

        SetVisibility(false);
    }

    private void OnCloseMenuInput(bool performed)
    {
        SetVisibility(false);
    }

    private void Update()
    {
        cg.alpha = Mathf.Lerp(cg.alpha, destAlpha, Time.deltaTime * 20f);
    }

    private void OnDestroy()
    {
        ServiceLocator.instance.GetService<InputManager>().OnCloseMenu -= OnCloseMenuInput;
    }

    public void SetVisibility(bool visible, bool restrictPlayerMovement = false)
    {
        destAlpha = visible ? 1f : 0f;
        cg.interactable = cg.blocksRaycasts = visible;

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MainMenu)
            if (ServiceLocator.instance.GetService<PlayerMovement>())
            {
                ServiceLocator.instance.GetService<PlayerMovement>().CanMove = !restrictPlayerMovement;

                Cursor.visible = restrictPlayerMovement;
                Cursor.lockState = restrictPlayerMovement ? CursorLockMode.None : CursorLockMode.Locked;
            }
    }
}
