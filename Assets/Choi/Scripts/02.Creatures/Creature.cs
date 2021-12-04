using System;
using UnityEngine;

/// <summary>
/// 크리쳐의 정보를 저장하는 클래스
/// </summary>
[Serializable]
public class Creature
{
    public float viewDistance = 10f;

    public float patrolRange = 100f;
    public float patrolSpeed = 2f;

    public float trackingRange = 60f;
    public float trackingSpeed = 4f;

    public float attackRange = 20f;
    public float attackDistance = 20f;

    public CreatureType type;
    public CreatureState state;
}

/// <summary>
/// 타입을 지정할 enum
/// </summary>
public enum CreatureType
{
    NormalType = 0,
}
/// <summary>
/// 크리쳐의 상태
/// </summary>
public enum CreatureState
{
    Patrol = 0,
    Chase,
    Attack,
}