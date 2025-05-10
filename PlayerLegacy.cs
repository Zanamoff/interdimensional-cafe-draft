// using NUnit.Framework.Internal;
// using UnityEngine;
// using UnityEngine.Rendering;

// public class Player : MonoBehaviour
// {
// [SerializeField] private float moveModifier = 7f;
// [SerializeField] private float sprintModifier = 2.5f;
// private bool isWalking;
// private bool isSprinting;
// private float moveSpeed;

//  private void Update()
//     {
//         moveSpeed = moveModifier;
//         Vector2 inputVector = new Vector2(0,0);
//         if (Input.GetKey(KeyCode.W)) {
//             inputVector.y = +1;
//             }
//         if (Input.GetKey(KeyCode.S)) {
//             inputVector.y = -1;
//             }
//         if (Input.GetKey(KeyCode.A)) {
//             inputVector.x = -1; 
//             }
//         if (Input.GetKey(KeyCode.D)) {
//             inputVector.x = +1; 
//             }
//         if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
//             moveSpeed = moveModifier*sprintModifier;
//         }
//         inputVector = inputVector.normalized;
        

//         Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
//         transform.position += moveDir * Time.deltaTime * moveSpeed; 
//         isWalking = moveDir != Vector3.zero;
//         isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

//         float rotateSpeed = 10f;
//         transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
//         }
//         public bool IsWalking(){
//             return isWalking;
//         }
//         public bool IsSprinting(){
//             return isSprinting;
//         }
//     }

