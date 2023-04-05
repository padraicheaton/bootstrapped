using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    private CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        ServiceLocator.instance.GetService<InputManager>().OnCloseMenu += performed => SetVisibility(false);

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

    public void SetVisibility(bool visible, bool restrictPlayerMovement = false)
    {
        cg.alpha = visible ? 1f : 0f;
        cg.interactable = cg.blocksRaycasts = visible;

        if (ServiceLocator.instance.GetService<PlayerMovement>())
        {
            ServiceLocator.instance.GetService<PlayerMovement>().CanMove = !restrictPlayerMovement;

            Cursor.visible = restrictPlayerMovement;
            Cursor.lockState = restrictPlayerMovement ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
