using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Fade in screen to scene view.
    /// </summary>
    /// <param name="time">Time to fade in.</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time)
    {
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

    /// <summary>
    /// Fade out screen to color of "Fader.Image" component.
    /// </summary>
    /// <param name="time">Time to fade out.</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time)
    {
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public void FadeOutImmediately()
    {
        _canvasGroup.alpha = 1;
    }
}
