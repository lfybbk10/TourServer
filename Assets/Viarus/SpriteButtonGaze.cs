using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
namespace Vrs.Internal
{
    public class SpriteButtonGaze : MonoBehaviour, IVrsGazeResponder
    {
        private bool mGazeAt = false;
        private Coroutine changeColorCoroutine;

        public void SetGazedAt(bool gazedAt)
        {
            if (mGazeAt != gazedAt)
            {
                mGazeAt = gazedAt;
                if (changeColorCoroutine != null)
                {
                    StopCoroutine(changeColorCoroutine);
                }
                Color targetColor = gazedAt ? new Color(0, 1, 0, 0.33f) : new Color(0, 0, 0, 0);
                changeColorCoroutine = StartCoroutine(ChangeColorGradually(targetColor));
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

        public bool isGazedAt()
        {
            return mGazeAt;
        }

        public void OnGazeEnter()
        {
            SetGazedAt(true);
        }

        public void OnGazeExit()
        {
            SetGazedAt(false);
        }

        public void OnGazeTrigger()
        {

        }

        public void OnUpdateIntersectionPosition(Vector3 position)
        {

        }

    }
}