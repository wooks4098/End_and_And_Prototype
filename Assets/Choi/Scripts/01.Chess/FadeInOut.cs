using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
	// CanvasGroup 컴포넌트의 알파값을 사용한다.
	// 페이드 인/아웃용 캔버스를 따로 만들었기 때문에 전체 값을 조절해도 된다.
	CanvasGroup canvasGroup;

	private void Awake()
    {
		canvasGroup = GetComponent<CanvasGroup>();
    }

	/// <summary>
	/// 페이드 인 코루틴
	/// 투명 -> 불투명	
	/// </summary>
	/// <param name="_fadeOutTime"></param>
	/// <returns></returns>
	public IEnumerator FadeInCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha < 1) // alpha is not1
		{
			// moving alpha toward 1
			// (Time.deltaTime / _fadeOutTime)만큼 값을 더해준다
			canvasGroup.alpha += Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}

	/// <summary>
	/// 페이드 아웃 코루틴
	/// 불투명 -> 투명
	/// </summary>
	/// <param name="_fadeOutTime"></param>
	/// <returns></returns>
	public IEnumerator FadeOutCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha > 0) // alpha is not0
		{
			// moving alpha toward 0
			// (Time.deltaTime / _fadeOutTime)만큼 값을 빼준다
			canvasGroup.alpha -= Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}
}
