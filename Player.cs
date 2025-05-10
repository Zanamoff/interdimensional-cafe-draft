using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class Player : MonoBehaviour, IKitchenObjectParent {
public static Player Instance { get; private set; }
public event EventHandler OnPickedSomething;
public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
public class OnSelectedCounterChangedEventArgs : EventArgs {
    public BaseCounter selectedCounter;
}

[SerializeField] private float moveSpeed = 7f;
[SerializeField] private GameInput gameInput;
[SerializeField] private LayerMask countersLayerMask;
[SerializeField] private Transform kitchenObjectHoldPoint;
private bool isWalking;
private Vector3 lastInteractDir;
private BaseCounter selectedCounter;
private KitchenObject kitchenObject;

public bool IsWalking(){
    return isWalking;
 }

private void Awake(){
    if (Instance != null) {
        Debug.LogError("There is more than one Player instance!");
    }
    Instance = this;
 }
 private void Start() {
    gameInput.OnInteractAction += GameInput_OnInteractAction;
    gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    gameInput.OnInteractAlternateCanceled += GameInput_OnInteractAlternateCanceled;
    gameInput.OnInteractAlternateActionContinuous += GameInput_OnInteractAlternateActionCont;
 }
 private void Update() {
        HandleMovement();
        HandleInteractions();
 }

 private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;

    if (selectedCounter != null) {
        selectedCounter.Interact(this);
    }
 }  
  private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;

    if (selectedCounter != null) {
        selectedCounter.InteractAlternate(this);
    }
 }
 private void GameInput_OnInteractAlternateActionCont (object sender, EventArgs e) {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;

    if (kitchenObject is FireExtinguisherKO fireExtinguisher){
        fireExtinguisher.Extinguish();
    } 
 }
 private void GameInput_OnInteractAlternateCanceled(object sender, EventArgs e) {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;
    
    if (kitchenObject is FireExtinguisherKO fireExtinguisher){
        fireExtinguisher.StopParticles();    
    } 
 }
    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)){
           if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
            //Has baseCounter
            if (baseCounter != selectedCounter){
                SetSelectedCounter(baseCounter);
            } 
           } else {
            SetSelectedCounter(null); 
           }
        } else {
            SetSelectedCounter(null);
        }
    }
    private void HandleMovement() {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return; //remake this later cause its better to just stop time completely  on GameOver
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float playerRadius = .5f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDir, moveDistance);
        if (canMove == false) {
            //Can't move towards moveDir
            //Attempt only X movement:
            Vector3 moveDirX = new Vector3 (moveDir.x, 0, 0).normalized;
             canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove == true) {
                //Can move only on the X
                moveDir = moveDirX;
            } else {
                //cant move on the X, attempt only Z movement:
                Vector3 moveDirZ = new Vector3 (0, 0, moveDir.z).normalized;
                 canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up*playerHeight, playerRadius, moveDirZ, moveDistance);
                 if (canMove == true) {
                    moveDir = moveDirZ;
                 }
            }
        }
        if (canMove == true) {
         transform.position += moveDir * moveDistance; 
        }

        isWalking = moveDir != Vector3.zero && canMove;
        
        float rotateSpeed = 15f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter){
     this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
                selectedCounter = selectedCounter
                });
    }
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject (KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    public void ClearKitchenObject() {
        kitchenObject = null;
    }
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}     
    

