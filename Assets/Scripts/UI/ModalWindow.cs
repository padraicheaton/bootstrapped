using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private AudioClip windowOpenClip;
    private AudioSource sfxAudioSource;
    protected CanvasGroup cg;
    protected float destAlpha;
    protected bool isVisible;

    protected virtual void Start()
    {
        if (windowOpenClip != null)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource.playOnAwake = false;
            sfxAudioSource.loop = false;
            sfxAudioSource.volume = 0.5f;

            sfxAudioSource.clip = windowOpenClip;
        }

        cg = GetComponent<CanvasGroup>();

        ServiceLocator.instance.GetService<InputManager>().OnCloseMenu += OnCloseMenuInput;

        for (int i = 0; i < transform.childCount; i++)
        {
            Button btn = transform.GetChild(i).GetComponent<Button>();

            if (btn && btn.name.Contains("Close"))
            {
                btn.onClick.AddListener(() => Hide());
                break;
            }
        }

        SetVisibility(false);
    }

    private void OnDisable()
    {
        if (ServiceLocator.instance != null)
            ServiceLocator.instance.GetService<InputManager>().OnCloseMenu -= OnCloseMenuInput;
    }

    protected virtual void OnCloseMenuInput(bool performed)
    {
        if (isVisible)
            Hide();
    }

    protected virtual void Hide()
    {
        SetVisibility(false);
    }

    private void Update()
    {
        cg.alpha = Mathf.Lerp(cg.alpha, destAlpha, Time.unscaledDeltaTime * 20f);
    }

    public void SetVisibility(bool visible, bool restrictPlayerMovement = false)
    {
        destAlpha = visible ? 1f : 0f;
        cg.interactable = cg.blocksRaycasts = visible;
        isVisible = visible;

        if (ServiceLocator.instance.GetService<SceneController>().GetActiveScene() != LoadedScenes.MainMenu)
        {
            if (ServiceLocator.instance.GetService<PlayerMovement>())
            {
                ServiceLocator.instance.GetService<PlayerMovement>().CanMove = !restrictPlayerMovement;

                Cursor.visible = restrictPlayerMovement;
                Cursor.lockState = restrictPlayerMovement ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        else
        {
            // If it is the main menu, always have the cursor visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (visible && windowOpenClip != null && !sfxAudioSource.isPlaying)
        {
            sfxAudioSource.Play();
        }
    }
}
