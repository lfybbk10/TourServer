using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Vrs.Internal
{
	public class SpriteButtonGaze : MonoBehaviour, IVrsGazeResponder
    {
		[SerializeField] private Image answerBackground;
		[SerializeField] private Zones zone;
		private TextMeshProUGUI textMesh;

		private bool mGazeAt = false;
		private Coroutine changeColorCoroutine;
		private Coroutine changeTextVisibilityCoroutine;
		private Coroutine changeBackgroundVisibilityCoroutine;


		public Action OnGazeEntered, OnGazeExited;


		private void OnEnable()
		{
			Init();
		}

        private void OnDisable()
        {
			Init();
        }

		private void Init()
		{ 
        textMesh = answerBackground.GetComponentInChildren<TextMeshProUGUI>();
			SetInitialTransparency();
			if (zone.currentMode == Mode.Exam)
			{
                Color targetColor = new Color(0, 0, 0, 0);
                GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = targetColor;
                Color startColor = textMesh.color;
                targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
                textMesh.color = targetColor;
                Color originalColor = answerBackground.color;
                targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
                answerBackground.color=targetColor;
			}
			else
			{
                Color targetColor = new Color(0, 1, 0, 0.33f);
                GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = targetColor;
                Color startColor = textMesh.color;
                targetColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
                textMesh.color = targetColor;
                Color originalColor = answerBackground.color;
                targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.78f);
                answerBackground.color = targetColor;
            }
		}

		private void SetInitialTransparency()
		{
			Color textColor = textMesh.color;
			textColor.a = 0f;
			textMesh.color = textColor;

			Color backgroundColor = answerBackground.color;
			backgroundColor.a = 0f;
			answerBackground.color = backgroundColor;
		}
		public void SetGazedAt(bool gazedAt)
		{
			if (mGazeAt != gazedAt)
			{
				mGazeAt = gazedAt;
				if (changeColorCoroutine != null)
				{
					StopCoroutine(changeColorCoroutine);
				}
				if (changeTextVisibilityCoroutine != null)
				{
					StopCoroutine(changeTextVisibilityCoroutine);
				}
				if (changeBackgroundVisibilityCoroutine != null)
				{
					StopCoroutine(changeBackgroundVisibilityCoroutine);
				}

				Color targetColor = gazedAt ? new Color(0, 1, 0, 0.33f) : new Color(0, 0, 0, 0);
				changeColorCoroutine = StartCoroutine(ChangeColorGradually(targetColor));
				float targetAlphaText = gazedAt ? 1.0f : 0f;
				changeTextVisibilityCoroutine = StartCoroutine(ChangeTextVisibilityGradually(targetAlphaText));
				float targetAlphaBackground = gazedAt ? answerBackground.color.a : 0f;
				changeBackgroundVisibilityCoroutine = StartCoroutine(ChangeBackgroundVisibilityGradually(targetAlphaBackground, gazedAt));
			}
		}

		private IEnumerator ChangeColorGradually(Color targetColor)
		{
			float duration = 0.33f;
			float elapsedTime = 0;
			SpriteShapeRenderer renderer = GetComponent<UnityEngine.U2D.SpriteShapeRenderer>();
			Color startColor = renderer.color;

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				renderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
				yield return null;
			}

			renderer.color = targetColor;
		}

		private IEnumerator ChangeTextVisibilityGradually(float targetAlpha)
		{
			float duration = 0.33f;
			float elapsedTime = 0;
			Color startColor = textMesh.color;
			Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				textMesh.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
				yield return null;
			}

			textMesh.color = targetColor;
		}

		private IEnumerator ChangeBackgroundVisibilityGradually(float targetAlpha, bool gazedAt)
		{
			float duration = 0.33f;
			float elapsedTime = 0;
			Color originalColor = answerBackground.color;
			Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, gazedAt ? 0.78f : 0f);

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				answerBackground.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
				yield return null;
			}

			answerBackground.color = targetColor;
		}


		public bool isGazedAt()
		{
			return mGazeAt;
		}

		public void OnGazeEnter()
		{
			if (zone.currentMode == Mode.Exam)
			{
				OnGazeEntered?.Invoke();
				SetGazedAt(true); 
			}
		}

		public void OnGazeExit()
		{
			if (zone.currentMode == Mode.Exam)
			{
				OnGazeExited?.Invoke();
				SetGazedAt(false); 
			}
		}

		public void OnGazeTrigger() { }

		public void OnUpdateIntersectionPosition(Vector3 position) { }

    }
}
