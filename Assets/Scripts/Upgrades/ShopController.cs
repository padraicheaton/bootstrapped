using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : Interactable
{
    [Header("References")]
    [SerializeField] private ModalWindow shopModal;

    public override string GetName()
    {
        return "Open";
    }
    public override void OnInteracted()
    {
        shopModal.SetVisibility(true, true);
    }
}
