using System;
using Mirror;
using UnityEngine;


public class ClientStudentManager : Singleton<ClientStudentManager>
{
    public Action<string> OnStudentNameChanged;
    
    public Student Student;

    public int id;

    public void CreateStudent()
    {
        gameObject.SetActive(true);
        Student = new Student(String.Empty, null);
    }

    public void SetNetworkConnection(NetworkConnection connection)
    {
        Student.Connection = connection;
    }
    
    public void SetStudentName(string studentName)
    {
        Student.Name = studentName;
        OnStudentNameChanged?.Invoke(studentName);
    }
}
