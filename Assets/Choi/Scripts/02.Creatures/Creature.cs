using System;
using UnityEngine;


[Serializable]
public class Creature
{
    public bool isActive;
    public float moveRange = 10f;
    public float moveSpeed = 2f;

    public CreatureType type;
}

public enum CreatureType
{
    NormalType,
}