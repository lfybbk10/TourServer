using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class CameraUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Text _nameText;

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void AddButtonListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
