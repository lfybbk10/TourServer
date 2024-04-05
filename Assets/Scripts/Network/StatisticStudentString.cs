using System;
using UnityEngine;
using UnityEngine.UI;


public class StatisticStudentString : MonoBehaviour
{
    public Student LinkedStudent;
    
    [SerializeField] private Text _name;
    [SerializeField] private Button _spectateButton;
    
    public event Action OnSpectateClick;

    private void Awake()
    {
        _spectateButton.onClick.AddListener((() => OnSpectateClick?.Invoke()));
    }

    public void SetName(string name)
    {
        _name.text = name;
    }
}
