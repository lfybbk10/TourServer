using UnityEngine;

public enum AnswerType
{
    SwitchVideo,
    ContinueVideo,
}

public class Answer : MonoBehaviour
{
    [SerializeField] private GameObject answerObject;
    [SerializeField] public AnswerType answerType;

    [HideInInspector] public Question parentQuestion;
    public bool isCorrect;

    public void ResponseProcess(Zones zone)
    {
        if (!isCorrect)
        {
            zone.MakeMistake();
            gameObject.SetActive(false);
            return;
        }
        parentQuestion.correctAnswers--;
        if (parentQuestion.correctAnswers > 0)
        {
            gameObject.SetActive(false);
            return;
        }
        parentQuestion.questionTextUI.text = "";
        zone.TrueAnswer();
        if (answerType == AnswerType.ContinueVideo)
        {
            
            zone.NextQuestion();
        }
        else
        {
            zone.NextStage();
        }
    }
}
