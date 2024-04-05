
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    private static readonly List<string> _names = new List<string>() {"Имя1", "Имя2", "Имя3", "Имя4"};

    public static string GetRandomName()
    {
        return _names[Random.Range(0, _names.Count)];
    }
}
