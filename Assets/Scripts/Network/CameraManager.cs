using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraManager : Singleton<CameraManager>
{
    public event Action OnCamsCountChanged;
    
    private readonly Dictionary<Student, CameraForServer> _dictCams = new Dictionary<Student, CameraForServer>();

    public readonly List<CameraForServer> Cams = new List<CameraForServer>();

    private void OnEnable()
    {
        ConnectedStudentsManager.Instance.OnStudentDisconnected += OnStudentDisconnected;
    }

    private void OnDisable()
    {
        ConnectedStudentsManager.Instance.OnStudentDisconnected -= OnStudentDisconnected;
    }

    public Student GetStudentByCam(CameraForServer cam)
    {
        foreach (var val in _dictCams)
        {
            if (val.Value == cam)
                return val.Key;
        }

        return null;
    }

    private void OnStudentDisconnected(Student student)
    {
        Destroy(_dictCams[student].gameObject);
        Cams.Remove(_dictCams[student]);
        _dictCams.Remove(student);
        OnCamsCountChanged?.Invoke();
    }

    public void AddCamera(Student student, CameraForServer cameraForServer)
    {
        print("add camera");
        _dictCams.Add(student, cameraForServer);
        Cams.Add(cameraForServer);
        OnCamsCountChanged?.Invoke();
    }

    public void SelectCamera(Student student)
    {
        foreach (var cam in FindObjectsOfType<CameraForServer>())
        {
            cam.gameObject.SetActive(false);
        }
        _dictCams[student].gameObject.SetActive(true);
        FindObjectOfType<ServerCamera>(true).gameObject.SetActive(false);
    }

    public CameraForServer GetCamera(Student student)
    {
        return _dictCams[student];
    }

    public void SelectServerCamera()
    {
        foreach (var cam in FindObjectsOfType<CameraForServer>())
        {
            cam.gameObject.SetActive(false);
        }
        FindObjectOfType<ServerCamera>(true).gameObject.SetActive(true);
    }
}