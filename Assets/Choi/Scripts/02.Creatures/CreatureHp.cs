using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ũ���� HP�� ����
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
    /// �������� �޴� ó��
    /// </summary>
    /// <param name="_value">������ ��</param>
    public void GetDamage(float _value)
    {
        currentHp = currentHp - _value;
    }

    /// <summary>
    ///  ���� ü��
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHp()
    {
        return currentHp;
    }
}
