using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 크리쳐 HP를 연산
/// </summary>
public class CreatureHp : MonoBehaviour
{
    // 체력
    public float maxHp = 100f;
    private float currentHp = 100f;

    // 체크용 bool 
    private bool isDead = false;
    public bool GetIsDead() { return isDead; }



    private void OnEnable()
    {
        ResetHp();
    }

    /// <summary>
    /// 죽었을 때 실행할 함수
    /// </summary>
    private void Die()
    {
        if (isDead) return;

        // 죽었음을 체크
        isDead = true;
        // 죽는 애니메이션 실행
        GetComponent<Animator>().SetTrigger("Die");
        // 현재 실행하던 행동 강제로 취소
        GetComponent<CreatureActionScheduler>().CancelCurrentAction();
    }

    /// <summary>
    /// HP를 최대HP로 재설정
    /// </summary>
    public void ResetHp()
    {
        currentHp = maxHp;
    }

    /// <summary>
    /// 데미지를 받는 처리
    /// </summary>
    /// <param name="_value">데미지 값</param>
    public void GetDamage(float _value)
    {
        currentHp = currentHp - _value;

        // 체력이 0 아래로 떨어졌을 경우
        if(currentHp <= 0)
        {
            // 사망
            Die();
        }
        else
        {
            // 피격

        }
    }

    /// <summary>
    ///  현재 체력 값을 외부에서 접근
    /// </summary>
    public float GetCurrentHp()
    {
        return currentHp;
    }
}
