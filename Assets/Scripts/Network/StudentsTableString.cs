using UnityEngine;
using UnityEngine.UI;


public class StudentsTableString : MonoBehaviour
{
    public Student LinkedStudent;
    [SerializeField] private InputField _nameField;
    [SerializeField] private Text _equalVersion;

    public void SetName(string name)
    {
        _nameField.text = name;
    }

    public void SetEqualVersions(bool isEqual)
    {
        _equalVersion.text = isEqual ? "Версия совпала" : "Версия не совпала";
    }

    public void SubscribeToChangeNameEvent(Student student)
    {
        _nameField.onEndEdit.AddListener((newName => student.Name = newName));    
    }
}
