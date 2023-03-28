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

    private void Awake()
    {
        input = new PlayerControls();
        inputActions = input.OnFoot;

        inputActions.Enable();

        inputActions.Fire.performed += ctxt => OnFireButton?.Invoke(true);
        inputActions.Fire.canceled += ctxt => OnFireButton?.Invoke(false);

        inputActions.Interact.performed += ctxt => OnInteractButton?.Invoke(true);
        inputActions.Interact.canceled += ctxt => OnInteractButton?.Invoke(false);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public Vector2 GetLookInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
