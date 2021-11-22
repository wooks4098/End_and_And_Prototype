using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
	CanvasGroup canvasGroup;
	Coroutine currentActiveFade = null;

	private void Awake()
    {
		canvasGroup = GetComponent<CanvasGroup>();
    }
	// 투명 -> 불투명
	public IEnumerator FadeInCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha < 1) // alpha is not1
		{
			// moving alpha toward 1
			canvasGroup.alpha += Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}

	// 불투명 -> 투명
	public IEnumerator FadeOutCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha > 0) // alpha is not0
		{
			// moving alpha toward 0
			canvasGroup.alpha -= Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}
}
