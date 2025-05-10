using UnityEngine;
using UnityEngine.Rendering;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSO;
    }
    public IKitchenObjectParent GetKitchenObjectParent(){
        return kitchenObjectParent;
    }
    public virtual void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent){
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
    this.kitchenObjectParent = kitchenObjectParent;
    if (kitchenObjectParent.HasKitchenObject()) {
        Debug.LogError("This parent already has a KitchenObject!");
    }
    kitchenObjectParent.SetKitchenObject(this);
    transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
    }
    public void DestroySelf(){
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject){
        if (this is PlateKitchenObject){
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }
    }
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent, Vector3? position = null, Quaternion? rotation = null){
     Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
     KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
     kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
     //optional position/rotation correction
      if (position.HasValue){
        kitchenObjectTransform.localPosition = position.Value;
     }
      if (rotation.HasValue){
        kitchenObjectTransform.localRotation = rotation.Value;
     }

     return kitchenObject;
    }

}
