using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionDistance;

    private Interactable highlightedInteractable;
    private InteractionPopup popup;

    private void Start()
    {
        ServiceLocator.instance.GetService<InputManager>().OnInteractButton += OnInteractInput;

        popup = ServiceLocator.instance.GetService<InteractionPopup>();
    }

    private void OnInteractInput(bool performed)
    {
        if (performed && highlightedInteractable)
        {
            highlightedInteractable.OnInteracted();
        }
    }

    private void Update()
    {
        highlightedInteractable = null;

        popup.SetVisibleState(false);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                if (!interactable.IsInteractable) return;

                interactable.OnHighlighted();

                highlightedInteractable = interactable;

                popup.SetVisibleState(true);
                popup.SetData(hit.collider.transform.position + Vector3.up * 0.75f, interactable);
            }
        }
    }
}
