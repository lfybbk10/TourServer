using System;
using System.Collections.Generic;
using UnityEngine;


public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private ClientStudentWindow _clientStudentWindow;
    [SerializeField] private GameObject _table;
    [SerializeField] private StudentsTableString _studentsTableStringPrefab;

    private List<StudentsTableString> _tableStrings = new List<StudentsTableString>();

    public void InitServerUI()
    {
        _table.SetActive(true);
    }

    public void InitClientUI()
    {
        _clientStudentWindow.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        ConnectedStudentsManager.Instance.OnNewStudentAdded += CreateTableString;
        ConnectedStudentsManager.Instance.OnStudentDisconnected += RemoveTableString;
    }

    private void OnDisable()
    {
        ConnectedStudentsManager.Instance.OnNewStudentAdded -= CreateTableString;
        ConnectedStudentsManager.Instance.OnStudentDisconnected -= RemoveTableString;
    }

    public void CreateTableString(Student student)
    {
        print("createtable string");
        var tableString = Instantiate(_studentsTableStringPrefab, Vector3.zero, Quaternion.identity, _table.transform);
        tableString.SetName(student.Name);
        tableString.SetEqualVersions(student.isEqualVersions());
        tableString.SubscribeToChangeNameEvent(student);
        tableString.LinkedStudent = student;
        _tableStrings.Add(tableString);
    }

    public void RemoveTableString(Student student)
    {
        print("remove table string");
        var tableString = _tableStrings.Find((s => s.LinkedStudent == student));
        if (tableString != null)
        {
            _tableStrings.Remove(tableString);
            Destroy(tableString.gameObject);
        }
    }
}
