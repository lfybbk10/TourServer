using UnityEngine;
using TMPro;
using System.Collections;

public class IPDAdjuster : MonoBehaviour
{
    public RectTransform leftEye;
    public RectTransform rightEye;
    public RectTransform eyeLine;
    public TextMeshProUGUI ipdText;
    public float adjustmentAmount = 1f; 
    public float eyeMoveAmount = 1.0f; 
    private float currentIPD = 58.0f;
    private float duration = 0.5f; 
    private float minIPD = 55.0f; 
    private float maxIPD = 72.0f;
    private Coroutine moveCoroutine;

    private Vector2 targetLeftEyePos;
    private Vector2 targetRightEyePos;
    private Vector2 targetLineSize;
    private Vector2 initialLineSize;
    private void Start()
    {
        initialLineSize = eyeLine.sizeDelta; 
        UpdateTargets(0);
    }

    private void MoveEyesAndLine(float eyeAdjustment)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        UpdateTargets(eyeAdjustment);

        moveCoroutine = StartCoroutine(MoveEyesAndLineRoutine());
    }

    private void UpdateTargets(float eyeAdjustment)
    {
        targetLeftEyePos += new Vector2(-eyeAdjustment, 0);
        targetRightEyePos += new Vector2(eyeAdjustment, 0);

        float totalAdjustment = (currentIPD - 58.0f) * eyeMoveAmount;
        targetLineSize = new Vector2(initialLineSize.x + totalAdjustment * 2, initialLineSize.y);
    }



    private IEnumerator MoveEyesAndLineRoutine()
    {
        float time = 0;
        Vector2 leftStartPos = leftEye.anchoredPosition;
        Vector2 rightStartPos = rightEye.anchoredPosition;
        Vector2 lineStartSize = eyeLine.sizeDelta;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            t = Mathf.Sin(t * Mathf.PI * 0.5f); 

            leftEye.anchoredPosition = Vector2.Lerp(leftStartPos, targetLeftEyePos, t);
            rightEye.anchoredPosition = Vector2.Lerp(rightStartPos, targetRightEyePos, t);
            eyeLine.sizeDelta = Vector2.Lerp(lineStartSize, targetLineSize, t);

            yield return null;
        }
        leftEye.anchoredPosition = targetLeftEyePos;
        rightEye.anchoredPosition = targetRightEyePos;
        eyeLine.sizeDelta = targetLineSize;
    }

    public void AdjustIPD(bool increase)
    {
        float newIPD = currentIPD + (increase ? adjustmentAmount : -adjustmentAmount);

        if (newIPD >= minIPD && newIPD <= maxIPD)
        {
            currentIPD = newIPD;
            float eyeAdjustment = increase ? eyeMoveAmount : -eyeMoveAmount;
            MoveEyesAndLine(eyeAdjustment);
        }

        ipdText.text = $"{currentIPD:F1} ��";
    }

    public void IncreaseIPD()
    {
        AdjustIPD(true);
    }

    public void DecreaseIPD()
    {
        AdjustIPD(false);
    }
}
