using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class ConnectedStudentsManager : Singleton<ConnectedStudentsManager>
{
    public event Action<Student> OnNewStudentAdded;
    public event Action<Student> OnStudentDisconnected;

    public event Action OnAllStudentsDisconnected;
    
    private StudentFactory _studentFactory;
    private readonly List<Student> _students = new List<Student>();
    

    private void Start()
    {
        _studentFactory = new StudentFactory();
    }

    public void AddConnectedStudent(NetworkConnectionToClient conn)
    {
        Student newStudent = _studentFactory.Create(conn);
        _students.Add(newStudent);
        OnNewStudentAdded?.Invoke(newStudent);
        StartCoroutine(NameCoroutine(conn, newStudent.Name));
    }

    private IEnumerator NameCoroutine(NetworkConnectionToClient conn, string name)
    {
        yield return new WaitForSeconds(1);
        conn.Send(new StudentNameMessage(){name = name});
    }

    public void RemoveStudent(NetworkConnectionToClient conn)
    {
        var student = _students.Find((student => student.Connection == conn));
        if (student != null)
        {
            _students.Remove(student);
            OnStudentDisconnected?.Invoke(student);
        }
        if(_students.Count==0)
            OnAllStudentsDisconnected?.Invoke();
    }

    public Student GetStudentByConnection(NetworkConnection connection)
    {
        return _students.Find((student => student.Connection == connection));
    }

    public List<Student> GetStudents() => _students;
}