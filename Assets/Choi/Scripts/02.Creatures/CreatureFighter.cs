using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    Animator animator;
    NavMeshAgent agent;
        
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    /* ============== üũ�� bool Ÿ�� ============== */
    // ���� ���ΰ�
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }


    /* ============== Ÿ�� ================ */
    // �ӽ� Ÿ��
    [SerializeField] CreaturePlayer tempTarget;
    // ���� Ÿ��
    [SerializeField] CreaturePlayer targetCharacter;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public void StartAttackBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);

        // Caster ������Ʈ�� ������� ������
        if(GetComponent<CreatureCaster>() != null)
        {
            // 
        }

        Attack();        
    }

    private void Attack()
    {
        isAttacking = true;
        if (targetCharacter == null) return;
        
        // �÷��̾ �ٶ󺸰�
        transform.LookAt(targetCharacter.transform);
        // �����
        agent.velocity = Vector3.zero;
        agent.isStopped = false;

        // �ִϸ�����
        animator.SetTrigger("Run Attack");
        // animator.ResetTrigger("Prepare Attack");

        if(targetCharacter != null)
        {
            // �����Ѵ�
            Debug.Log("AttackBehaviour()");
        }
    }



    public void Cancel()
    {

    }
}
