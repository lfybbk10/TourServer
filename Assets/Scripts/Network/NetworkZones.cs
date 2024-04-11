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

    [Command]
    private void ChooseAnswer(AnswerType answerType, bool isCorrect)
    {
        if (!isCorrect)
        {
            _zones.MakeMistake();
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
