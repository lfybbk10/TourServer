using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RenderHeads.Media.AVProVideo;
using Vrs.Internal;
using System;

public class Zones : MonoBehaviour
{
    [SerializeField] private MediaPlayer mediaPlayer;
    [SerializeField] private List<Stage> stages = new List<Stage>();
    [SerializeField] private TMP_Text allQuestionsText;
    [SerializeField] private TMP_Text wrongAnswersText;
    [SerializeField] private TMP_Text mainQuestion;
    [SerializeField] private GameObject modePanel;

    private int currentStageIndex;
    private int currentQuestionIndex;
    private int wrongAnswers = 0;
    private int allAnswers = 0,currentAnswers = 0;
    private float nextQuestionTime = 100;
    private string videoFolderPath;
    private bool isLastAnswer = false, isStarted = false;

    public Action<AnswerType, bool, int> OnChoosedAnswer;

    private bool haveDoneMistake = false;


    [HideInInspector] public Mode currentMode;

    private void Start()
    {
        currentStageIndex = 0;
        currentQuestionIndex = 0;
        mainQuestion.text = "";
    }

    public void OpenModePanel()
    {
        modePanel.SetActive(true);
        if (mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Pause();
        }
    }

    public void SelectMode(int selectedMode)
    {
        currentMode = (Mode)selectedMode;
        modePanel.SetActive(false);
        if (mediaPlayer.Control.GetCurrentTime() < nextQuestionTime && !isLastAnswer)
        {
            if (!isStarted)
            {
                isStarted = true;
                PlayVideo();
            }
            else
            {
                mediaPlayer.Play();
            }
        }
        else if (!isStarted)
        {
            isStarted = true;
            PlayVideo();
        }
        else if(!isLastAnswer)
        {
            PauseVideo();
        }
        else
        {
            mediaPlayer.Play();
        }

    }

    private void PlayVideo()
    {
#if UNITY_EDITOR
        videoFolderPath = Application.dataPath + "/TigerVideos/";
#else
videoFolderPath = "storage/emulated/0/TigerVideos/";
#endif

        var videoPath = videoFolderPath + stages[currentStageIndex].videoCaption;
        mediaPlayer.OpenMedia(new MediaPath(videoPath, MediaPathType.AbsolutePathOrURL));
        nextQuestionTime = stages[currentStageIndex].GetNextQuestionTime(currentQuestionIndex);
        Vector3 eulerRotation = new Vector3(0, stages[currentStageIndex].GetStartAngle(), 0);
        mediaPlayer.transform.rotation = Quaternion.Euler(eulerRotation);
        CountAnswers();
        mediaPlayer.Play();
    }


    private void PauseVideo()
    {
        mediaPlayer.Pause();
        stages[currentStageIndex].HideQuestion(currentQuestionIndex);
        stages[currentStageIndex].gameObject.SetActive(true);
        if (currentMode == Mode.Study)
        {
            stages[currentStageIndex].ShowAnswer(currentQuestionIndex);
        }
        else
        {
            stages[currentStageIndex].ShowQuestion(currentQuestionIndex);
        }
    }

    private void CountAnswers()
    {
        if (allAnswers == 0)
        {
            foreach (var stage in stages)
            {
                allAnswers += stage.GetQuestionsCount();
            }
        }
        allQuestionsText.text = "Ответов: " + currentAnswers + "/" + allAnswers;
    }

    private void ReturnVideo()
    {
        CountAnswers();
        mediaPlayer.Play();
    }


    public void ChooseAnswer(Answer answer)
    {
        print("Выбран ответ"+answer.gameObject.name);
        OnChoosedAnswer?.Invoke(answer.answerType, answer.isCorrect, answer.parentQuestion.GetIndexByAnswer(answer));
        answer.ResponseProcess(this);
    }


    public void MakeMistake()
    {
            wrongAnswers++;
            wrongAnswersText.text = "\nОшибок: " + wrongAnswers;
    }

    public void TrueAnswer()
    {
        currentAnswers++;
    }


    public void NextQuestion()
    {
        if (currentQuestionIndex < stages[currentStageIndex].QuestionCount() - 1)
        {
            stages[currentStageIndex].HideQuestion(currentQuestionIndex);
            currentQuestionIndex++;
            nextQuestionTime = stages[currentStageIndex].GetNextQuestionTime(currentQuestionIndex);

        }
        else
        {
            isLastAnswer = true;
        }
        stages[currentStageIndex].gameObject.SetActive(false);
        ReturnVideo();
    }


    public void NextStage()
    {
        if (currentStageIndex < stages.Count - 1)
        {
            mediaPlayer.CloseMedia();
            stages[currentStageIndex].gameObject.SetActive(false);
            currentStageIndex++;
            currentQuestionIndex = 0;
            isLastAnswer = false;
            PlayVideo();
        }
        else
        {
            Debug.Log("Выход в меню");
        }
    }


    private void Update()
    {
        try
        {
            if (!mediaPlayer.Control.IsPaused() && mediaPlayer.Control.GetCurrentTime() >= nextQuestionTime && !isLastAnswer)
            {
                PauseVideo();
            }
            if (mediaPlayer.Control.IsFinished())
            {
                NextStage();
            }
        }
        catch(Exception e)
        {
            FPScounter.Print(e.ToString());
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    public Question GetCurrentQuestion() => stages[currentStageIndex].GetQuestionAt(currentQuestionIndex);

    
    public void Restart()
    {
        isLastAnswer = false;
        mediaPlayer.Pause();
        stages[currentStageIndex].HideQuestion(currentQuestionIndex);
        currentAnswers = 0;
        currentQuestionIndex = 0;
        currentStageIndex = 0;
        wrongAnswers = 0;
        wrongAnswersText.text = "\nОшибок: " + wrongAnswers;
        allQuestionsText.text = "Ответов: " + currentAnswers + "/" + allAnswers;
        foreach(var stage in stages)
        {
            stage.gameObject.SetActive(false);
        }
        isStarted = false;
        OpenModePanel();
        mainQuestion.text = "";
    }
}

public enum Mode
{
    Study,
    Test,
    Exam
}

