using System;
using Mirror;
using UnityEngine;

public class Student
{
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            OnNameChanged?.Invoke(value);
            _name = value;
        }
    }

    public event Action<string> OnNameChanged;

    public NetworkConnection Connection { get; set; }

    public Student(string name, NetworkConnection connection)
    {
        Connection = connection;
        Name = name;
    }

    public bool isEqualVersions() => true;
}
