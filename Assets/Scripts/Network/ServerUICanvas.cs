using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ServerUICanvas : Singleton<ServerUICanvas>
{
    [SerializeField] private Button _ghostMode, _statisticMode, _multipleCamsMode;
    [SerializeField] private Image _modeButtonsBg;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject _statisticWindowPrefab;
    [SerializeField] private StatisticStudentString _statisticStudentStringPrefab;
    [SerializeField] private Canvas _ghostModeCanvas;
    public Canvas MultipleCamsCanvas;

    private GameObject _statisticWindow;

    private List<StatisticStudentString> _studentStrings = new List<StatisticStudentString>();
    
    protected override void Awake()
    {
        base.Awake();
        InitStatisticWindow();
        StartMultipleMode();
    }

    private void OnEnable()
    {
        ConnectedStudentsManager.Instance.OnStudentDisconnected += OnStudentDisconnected;
    }

    private void OnDisable()
    {
        ConnectedStudentsManager.Instance.OnStudentDisconnected -= OnStudentDisconnected;
    }

    private void InitStatisticWindow()
    {
        _statisticWindow = Instantiate(_statisticWindowPrefab);
        _statisticWindow.transform.SetParent(ServerUICanvas.Instance.transform,false);
        _statisticWindow.transform.SetSiblingIndex(0);
        AddConnectedStudents();
        _statisticWindow.gameObject.SetActive(false);
    }

    private void AddConnectedStudents()
    {
        foreach (var student in ConnectedStudentsManager.Instance.GetStudents())
        {
            var studentString = Instantiate(_statisticStudentStringPrefab, Vector3.zero, Quaternion.identity,
                _statisticWindow.transform);
            studentString.SetName(student.Name);
            studentString.OnSpectateClick += (() => OnSpectateClick(student));
            studentString.LinkedStudent = student;
            _studentStrings.Add(studentString);
        }
    }

    private void OnStudentDisconnected(Student student)
    {
        print("disconeect");
        var studentString = _studentStrings.Find((s => s.LinkedStudent == student));
        if (studentString != null)
        {
            print("disconeect remove");
            _studentStrings.Remove(studentString);
            Destroy(studentString.gameObject);
        }
    }

    private void OnSpectateClick(Student student)
    {
        _statisticWindow.SetActive(false);
        cameraManager.SelectCamera(student);
        _statisticMode.gameObject.SetActive(true);
        MultipleCamsCanvas.gameObject.SetActive(false);
    }

    public void OnGhostModeClick()
    {
        //_ghostModeController.IsGhostModeEnabled = true;
        _ghostModeCanvas.gameObject.SetActive(true);
        _statisticWindow.SetActive(false);
        _statisticMode.gameObject.SetActive(true);
        _ghostMode.gameObject.SetActive(false);
        _multipleCamsMode.gameObject.SetActive(false);
    }

    public void OnStatisticModeClick()
    {
        _statisticWindow.SetActive(true);
        cameraManager.SelectServerCamera();
        _statisticMode.gameObject.SetActive(false);
        _modeButtonsBg.gameObject.SetActive(false);
        _multipleCamsMode.gameObject.SetActive(true);
        _ghostMode.gameObject.SetActive(true);
        MultipleCamsCanvas.gameObject.SetActive(false);
        //_ghostModeController.IsGhostModeEnabled = false;
    }

    public void StartMultipleMode()
    {
        _statisticMode.gameObject.SetActive(true);
        _statisticWindow.SetActive(false);
        _multipleCamsMode.gameObject.SetActive(false);
        _modeButtonsBg.gameObject.SetActive(true);
        MultipleCamsCanvas.gameObject.SetActive(true);
        _ghostMode.gameObject.SetActive(false);
        print("start multi mode");
        MultipleCamerasController.Instance.CalculateCamsRect();
    }

    public void ShowMultipleButton()
    {
        MultipleCamsCanvas.gameObject.SetActive(false);
        _modeButtonsBg.gameObject.SetActive(false);
        _multipleCamsMode.gameObject.SetActive(true);
    }

    public void Quit()
    {
        ServerChangeSceneController.Instance.ReturnToLobby();
    }
}
