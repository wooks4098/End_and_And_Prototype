using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Creature", menuName ="ScriptableObjects/Creature")]
public class CreatureSO : ScriptableObject
{
    [SerializeField] float viewDistance = 10f;
    
    [SerializeField] float patrolRange = 100f;
    [SerializeField] float patrolSpeed = 2f;
    
    [SerializeField] float trackingRange = 60f;
    [SerializeField] float trackingSpeed = 4f;

    [SerializeField] float attackRange = 20f;

    [SerializeField] bool hasTarget = false;

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
}