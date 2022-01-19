using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    // ÄÄÆ÷³ÍÆ®
    Animator animator;
    NavMeshAgent agent;
        
    [SerializeField] Creature creature;

    


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public void StartAttackBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
    }

    public void Cancel()
    {
        // StopAttack();
        // target = null;
        GetComponent<CreatureTracker>().Cancel();
    }




}
