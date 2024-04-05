// using System;
// using Mirror;
// using UnityEngine;
//
//
// public class GhostModeController : MonoBehaviour
// {
//     private float moveSpeed = 10f;
//     private float rotationSpeed = 1f;
//
//     [SerializeField] private Joystick movementJoystick;
//     [SerializeField] private Joystick rotationJoystick;
//
//     private Vector3 moveInput;
//     private Vector2 rotationInput;
//
//
//     public bool IsGhostModeEnabled { get; set; }
//     
//     private void Start()
//     {
//         var droneCam = FindObjectOfType<NetworkStartPosition>(true);
//         if (droneCam != null)
//         {
//             transform.position = droneCam.transform.position;
//         }
//     }
//
//     void Update()
//     {
//         if(!IsGhostModeEnabled)
//             return;
//         
//         moveInput = new Vector3(movementJoystick.Horizontal, 0f, movementJoystick.Vertical);
//         transform.Translate(moveInput * moveSpeed * Time.deltaTime);
//     }
//
//     void LateUpdate()
//     {
//         if(!IsGhostModeEnabled)
//             return;
//
//         rotationInput = new Vector2(rotationJoystick.Horizontal, rotationJoystick.Vertical);
//
//         float rotationX = rotationInput.y * rotationSpeed;
//         float rotationY = rotationInput.x * rotationSpeed;
//
//         transform.Rotate(Vector3.right, -rotationX, Space.Self);
//         transform.Rotate(Vector3.up, rotationY, Space.World);
//     }
// }
