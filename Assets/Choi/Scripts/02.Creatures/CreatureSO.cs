using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Creature", menuName ="ScriptableObjects/Creature")]
public class CreatureSO : ScriptableObject
{
    // �þ� (������ �� ��)
    [SerializeField] float viewDistance = 10f;
    
    // ��Ʈ�� ����
    [SerializeField] float patrolRange = 20f;
    // ��Ʈ�� �ӵ�
    [SerializeField] float patrolSpeed = 3f;
    
    // �߰� ����
    [SerializeField] float trackingRange = 12f;
    // �߰� �ӵ�
    [SerializeField] float trackingSpeed = 5f;

    // ���� ����
    [SerializeField] float attackRange = 4f;
    // ������ ��
    [SerializeField] float damage = 20f;

    // ũ���� Ÿ�� (������ �� ��)
    [SerializeField] CreatureType type;


    public float GetViewDistance()
    {
        return viewDistance;
    }
    public float GetPatrolRange()
    {
        return patrolRange;
    }
    public float GetPatrolSpeed()
    {
        return patrolSpeed;
    }
    public float GetTrackingRange()
    {
        return trackingRange;
    }
    public float GetTrackingSpeed()
    {
        return trackingSpeed;
    }
    public float GetAttackRange()
    {
        return attackRange;
    }
    
    public float GetDamageValue()
    {
        return damage;
    }
}