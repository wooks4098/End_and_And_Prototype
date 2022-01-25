using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크리쳐 HP를 연산
/// </summary>
public class CreatureHp : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp = 100f;


    private void OnEnable()
    {
        currentHp = maxHp;
    }

    public void ResetHp()
    {
        currentHp = maxHp;
    }

    /// <summary>
    /// 데미지를 받는 처리
    /// </summary>
    /// <param name="_value">데미지 값</param>
    public float GetDamage(float _value)
    {
        currentHp = currentHp - _value;

        return currentHp;
    }
}
