using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ũ���� ü��(HP)�� ���� ���� �з�
/// </summary>
public enum CreatureHpState
{
    Normal      = 0,    // ���� (�Ϲ�) : ~ 30%
    Arousal,            // ���� ���� : 30% ~ 10%
    Lull,               // �Ұ� ���� : 10% ~ 5%
    Vaccinable,         // ����� ���� �� �ִ� ���� : 5% ~
}

/// <summary>
/// ũ���� HP�� ����
/// </summary>
public class CreatureHp : MonoBehaviour
{
    // �ִ�ü��
    private float maxHp = 100f;
    // ����ü�� ... �׽�Ʈ������ �����̴�(Range) ���
    [Range(0,100)][SerializeField]
    float currentHp = 100f;
    public float GetCurrentHp() { return currentHp; }

    [SerializeField] CreatureHpState hpState;
    public CreatureHpState GetCurrentCreatureHPState() { return hpState; }

    #region Enable
    private void OnEnable()
    {
        // ü�� ����
        ResetHp();
        // ���� �Ϲ����� ����
        hpState = CreatureHpState.Normal;
    }
    #endregion


    private void Update()
    {
        // ���� ����
        SetCreatureStateAboutHp();
    }


    private void SetCreatureStateAboutHp()
    {
        if (currentHp > 30.0f)
        {
            hpState = CreatureHpState.Normal;
        }
        else if (currentHp <= 30.0f && currentHp > 10.0f)
        {
            hpState = CreatureHpState.Arousal;
        }
        else if (currentHp <= 10.0f && currentHp > 5.0f)
        {
            hpState = CreatureHpState.Lull;
        }
        else if (currentHp <= 5.0f)
        {
            hpState = CreatureHpState.Vaccinable;
        }
    }

    public void ResetHp()
    {
        currentHp = maxHp;
    }

    /// <summary>
    /// �������� �޴� ó��
    /// �������� ���� �� SetCreatureStateAboutHP() �޼��� ȣ�� (���� �ּ�ó�� �صΰ� ������Ʈ���� ȣ��)
    /// </summary>
    /// <param name="_value">������ ��</param>
    public float GetDamage(float _value)
    {
        currentHp = currentHp - _value;

        // ü��(Hp)�� ���� ���� ����
        // SetCreatureStateAboutHp();

        return currentHp;
    }
}
