using System;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class GameInput : MonoBehaviour {
    public static GameInput Instance {get; private set;}
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractAlternateActionContinuous;
    public event EventHandler OnInteractAlternateCanceled;
    public event EventHandler OnPauseAction;

  
  
    private void Awake(){
        if (Instance != null) {
            Debug.LogError("There is more than one GameInput Instance!");
        }
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.InteractAlternate.canceled += InteractAlternate_canceled;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Update(){
        if (playerInputActions.Player.InteractAlternate.IsPressed()){
            OnInteractAlternateActionContinuous?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnDestroy(){
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.InteractAlternate.canceled -= InteractAlternate_canceled;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        
        playerInputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);

    }
    private void InteractAlternate_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnInteractAlternateCanceled?.Invoke(this, EventArgs.Empty);

    }


    public Vector2 GetMovementVectorNormalized(){
         Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
