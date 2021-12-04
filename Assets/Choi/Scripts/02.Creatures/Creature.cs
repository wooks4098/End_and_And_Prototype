using System;
using UnityEngine;

/// <summary>
/// ũ������ ������ �����ϴ� Ŭ����
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
/// Ÿ���� ������ enum
/// </summary>
public enum CreatureType
{
    NormalType = 0,
}
/// <summary>
/// ũ������ ����
/// </summary>
public enum CreatureState
{
    Patrol = 0,
    Chase,
    Attack,
}