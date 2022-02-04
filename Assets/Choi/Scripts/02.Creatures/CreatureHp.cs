using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크리쳐 체력(HP)에 따른 상태 분류
/// </summary>
public enum CreatureHPState
{
    Normal      = 0,    // 보통 (일반) : ~ 30%
    Arousal,            // 각성 상태 : 30% ~ 10%
    Lull,               // 소강 상태 : 10% ~ 5%
    Vaccinable,         // 백신을 맞힐 수 있는 상태 : 5% ~
}

/// <summary>
/// 크리쳐 HP를 연산
/// </summary>
public class CreatureHp : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp = 100f;

    [SerializeField] CreatureHPState hpState;

    #region Enable
    private void OnEnable()
    {
        ResetHp();
    }
    #endregion


    private void SetCreatureStateAboutHp()
    {
        if (currentHp > 30.0f)
        {
            hpState = CreatureHPState.Normal;
        }
        else if (currentHp <= 30.0f && currentHp > 10.0f)
        {
            hpState = CreatureHPState.Arousal;
        }
        else if (currentHp <= 10.0f && currentHp > 5.0f)
        {
            hpState = CreatureHPState.Lull;
        }
        else if (currentHp <= 5.0f)
        {
            hpState = CreatureHPState.Vaccinable;
        }
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
