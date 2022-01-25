using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Creature", menuName ="ScriptableObjects/Creature")]
public class CreatureSO : ScriptableObject
{
    // 시야 (지금은 안 씀)
    [SerializeField] float viewDistance = 10f;
    
    // 패트롤 범위
    [SerializeField] float patrolRange = 20f;
    // 패트롤 속도
    [SerializeField] float patrolSpeed = 3f;
    
    // 추격 범위
    [SerializeField] float trackingRange = 12f;
    // 추격 속도
    [SerializeField] float trackingSpeed = 5f;

    // 공격 범위
    [SerializeField] float attackRange = 4f;
    // 데미지 값
    [SerializeField] float damage = 20f;

    // 크리쳐 타입 (지금은 안 씀)
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