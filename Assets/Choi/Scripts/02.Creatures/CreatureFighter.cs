using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    Animator animator;
    NavMeshAgent agent;    

    [SerializeField] GameObject goCastingProjector; // ĳ���õ��� ������ ǥ���� ��������
    [SerializeField] Creature creature;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public void Cancel()
    {
        
    }

    public void OnCastingProjector()
    {
        //animator.SetTrigger("Run Attack");
        goCastingProjector.SetActive(true);
    }

    public void OffCastingProjector()
    {
        goCastingProjector.SetActive(false);

        //animator.ResetTrigger("Prepare Attack");
        GetComponent<CreatureMovement>().IsCasting = false;
        GetComponent<CreatureMovement>().CanAttack = true;
        // creature.state = CreatureState.Attack;

        //animator.SetTrigger("Run Attack");
    }
}
