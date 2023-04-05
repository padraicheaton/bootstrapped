using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private PlayerControls input;
    private PlayerControls.OnFootActions inputActions;

    //* Control Events
    public UnityAction<bool> OnFireButton;
    public UnityAction<bool> OnInteractButton;
    public UnityAction<bool> OnDropWeapon;
    public UnityAction<bool> OnCloseMenu;
    public UnityAction<bool> OnAimButton;

    private void Awake()
    {
        input = new PlayerControls();
        inputActions = input.OnFoot;

        inputActions.Enable();

        inputActions.Fire.performed += ctxt => OnFireButton?.Invoke(true);
        inputActions.Fire.canceled += ctxt => OnFireButton?.Invoke(false);

        inputActions.Interact.performed += ctxt => OnInteractButton?.Invoke(true);
        inputActions.Interact.canceled += ctxt => OnInteractButton?.Invoke(false);

        inputActions.Drop.performed += ctxt => OnDropWeapon?.Invoke(true);
        inputActions.Drop.canceled += ctxt => OnDropWeapon?.Invoke(false);

        inputActions.CloseMenu.performed += ctxt => OnCloseMenu?.Invoke(true);
        inputActions.CloseMenu.canceled += ctxt => OnCloseMenu?.Invoke(false);

        inputActions.Aim.performed += ctxt => OnAimButton?.Invoke(true);
        inputActions.Aim.canceled += ctxt => OnAimButton?.Invoke(false);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public Vector2 GetMovementInput()
    {
        return inputActions.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLookInput()
    {
        return inputActions.Look.ReadValue<Vector2>();
    }

    public float GetScrollInput()
    {
        return inputActions.SwapWeapons.ReadValue<float>();
    }
}
