using System;
using Mirror;
using UnityEngine;


public class NetworkZones : NetworkBehaviour
{
    [SerializeField] private Zones _zones;

    private void OnEnable()
    {
        _zones.OnChoosedAnswer += ChooseAnswer;
    }

    private void OnDisable()
    {
        _zones.OnChoosedAnswer -= ChooseAnswer;
    }

    private void Start()
    {
        _zones.SelectMode(NetworkGameMode.Instance.GameMode);
    }

    private void SelectMode(int mode)
    {
        _zones.SelectMode(mode);
    }

    [Command]
    private void ChooseAnswer(AnswerType answerType, bool isCorrect, int index)
    {
        if (!isCorrect)
        {
            _zones.MakeMistake();
            _zones.GetCurrentQuestion().GetAnswerByIndex(index).gameObject.SetActive(false);
            return;
        }
        _zones.GetCurrentQuestion().correctAnswers--;
        if (_zones.GetCurrentQuestion().correctAnswers > 0)
        {
            _zones.GetCurrentQuestion().GetAnswerByIndex(index).gameObject.SetActive(false);
            return;
        }
        _zones.TrueAnswer();
        if (answerType == AnswerType.ContinueVideo)
        {
            _zones.NextQuestion();
        }
        else
        {
            _zones.NextStage();
        }
    }
}
