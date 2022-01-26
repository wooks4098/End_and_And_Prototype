using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ũ���� HP�� ����
/// </summary>
public class CreatureHp : MonoBehaviour
{
    // ü��
    public float maxHp = 100f;
    private float currentHp = 100f;

    // üũ�� bool 
    private bool isDead = false;
    public bool GetIsDead() { return isDead; }



    private void OnEnable()
    {
        ResetHp();
    }

    /// <summary>
    /// �׾��� �� ������ �Լ�
    /// </summary>
    private void Die()
    {
        if (isDead) return;

        // �׾����� üũ
        isDead = true;
        // �״� �ִϸ��̼� ����
        GetComponent<Animator>().SetTrigger("Die");
        // ���� �����ϴ� �ൿ ������ ���
        GetComponent<CreatureActionScheduler>().CancelCurrentAction();
    }

    /// <summary>
    /// HP�� �ִ�HP�� �缳��
    /// </summary>
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

        // ü���� 0 �Ʒ��� �������� ���
        if(currentHp <= 0)
        {
            // ���
            Die();
        }
        else
        {
            // �ǰ�

        }
    }

    /// <summary>
    ///  ���� ü�� ���� �ܺο��� ����
    /// </summary>
    public float GetCurrentHp()
    {
        return currentHp;
    }
}
