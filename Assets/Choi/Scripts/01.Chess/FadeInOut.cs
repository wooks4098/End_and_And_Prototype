using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
	// CanvasGroup ������Ʈ�� ���İ��� ����Ѵ�.
	// ���̵� ��/�ƿ��� ĵ������ ���� ������� ������ ��ü ���� �����ص� �ȴ�.
	CanvasGroup canvasGroup;

	private void Awake()
    {
		canvasGroup = GetComponent<CanvasGroup>();
    }

	/// <summary>
	/// ���̵� �� �ڷ�ƾ
	/// ���� -> ������	
	/// </summary>
	/// <param name="_fadeOutTime"></param>
	/// <returns></returns>
	public IEnumerator FadeInCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha < 1) // alpha is not1
		{
			// moving alpha toward 1
			// (Time.deltaTime / _fadeOutTime)��ŭ ���� �����ش�
			canvasGroup.alpha += Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}

	/// <summary>
	/// ���̵� �ƿ� �ڷ�ƾ
	/// ������ -> ����
	/// </summary>
	/// <param name="_fadeOutTime"></param>
	/// <returns></returns>
	public IEnumerator FadeOutCoroutine(float _fadeOutTime)
	{
		while (canvasGroup.alpha > 0) // alpha is not0
		{
			// moving alpha toward 0
			// (Time.deltaTime / _fadeOutTime)��ŭ ���� ���ش�
			canvasGroup.alpha -= Time.deltaTime / _fadeOutTime;
			yield return null;
		}
	}
}
