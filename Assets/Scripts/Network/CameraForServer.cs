using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using Vrs.Internal;


public class CameraForServer : NetworkBehaviour
{
    private GameObject _mainCamera;

    private Camera _cameraComponent;

    private List<CubeButtonGaze> _activeElements = new List<CubeButtonGaze>();

    private void Start()
    {
        _mainCamera = GameObject.Find("MainCamera");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        _cameraComponent = GetComponent<Camera>();
        _cameraComponent.enabled = true;
        print("start camera server");
        CameraManager.Instance.AddCamera(ConnectedStudentsManager.Instance.GetStudentByConnection(connectionToClient), this);
    }

    [Command]
    private void CmdClickOnServer()
    {
        _activeElements.ForEach((gaze => gaze.GetComponent<EventTrigger>().OnPointerClick(null)));
    }

    private void Update()
    {
        // if (NetworkServer.active)
        // {
        //     Vector3 centerOfScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        //
        //     Ray ray = _cameraComponent.ScreenPointToRay(centerOfScreen);
        //
        //     RaycastHit hitInfo;
        //     if (Physics.Raycast(ray, out hitInfo))
        //     {
        //         CubeButtonGaze gazeButton = hitInfo.collider.gameObject.GetComponent<CubeButtonGaze>();
        //         if (gazeButton!=null)
        //         {
        //             gazeButton.OnGazeEnter();
        //             _activeElements.Add(gazeButton);
        //         }
        //         else
        //         {
        //             _activeElements.ForEach((gaze => gaze.OnGazeExit()));
        //             _activeElements.Clear();
        //         }
        //     }
        //     else
        //     {
        //         _activeElements.ForEach((gaze => gaze.OnGazeExit()));
        //         _activeElements.Clear();
        //     }
        // }
    }
}