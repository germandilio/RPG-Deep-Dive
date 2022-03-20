using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        
        private Coroutine _activeFadeRoutine;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Fade in screen to scene view.
        /// </summary>
        /// <param name="time">Time to fade in.</param>
        /// <returns></returns>
        public Coroutine FadeIn(float time) => Fade(0f, time);

        /// <summary>
        /// Fade out screen to color of "Fader.Image" component.
        /// </summary>
        /// <param name="time">Time to fade out.</param>
        /// <returns></returns>
        public Coroutine FadeOut(float time) => Fade(1f, time);

        public void FadeOutImmediately()
        {
            _canvasGroup.alpha = 1;
        }

        private Coroutine Fade(float target, float time)
        {
            if (_activeFadeRoutine != null)
                StopCoroutine(_activeFadeRoutine);

            _activeFadeRoutine = StartCoroutine(FadeRoutine(target, time));
            return _activeFadeRoutine;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            } 
        }
    }
}