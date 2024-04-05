using System;
using UnityEngine;
using UnityEngine.UI;


public class ClientStudentWindow : MonoBehaviour
{
    [SerializeField] private Text _name;

    private void OnEnable()
    {
        ClientStudentManager.Instance.OnStudentNameChanged += SetText;
    }

    private void OnDisable()
    {
        ClientStudentManager.Instance.OnStudentNameChanged -= SetText;
    }

    private void SetText(string name)
    {
        _name.text = "Ваше имя: " + name;
    }
}
