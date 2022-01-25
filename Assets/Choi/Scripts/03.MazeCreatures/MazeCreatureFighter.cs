using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureFighter : MonoBehaviour, ICreatureAction
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

        Attack();
    }
    private void Attack()
    {
        // ���� ������ ǥ��
        isAttacking = true;

        // �ִϸ�����
        animator.SetTrigger("Try Attack");
        // ������Ʈ
        agent.isStopped = true;

        // Ÿ���� ã�´�
        FindTargetsForAttack();

        // Ÿ���� ������� ������
        if (targetCharacter != null)
        {
            // �������� �ش�
            Debug.Log("AttackBehaviour()");
        }

        // ������ �� ���ٰ� üũ
        GetComponent<MazeCreatureController>().CanAttack = false;
    }

    /// <summary>
    /// ������ Ÿ�� ĳ���� ã��
    /// </summary>
    private void FindTargetsForAttack()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetAttackRange());

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("���� ã�ҽ��ϴ�!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
            if (activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {
                // Ÿ�� ����
                targetCharacter = activeCollider.GetComponent<CreaturePlayer>();
            }
        }
    }

    #region call in attack animation 
    /// <summary>
    /// �ִϸ��̼� 
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;

        if (targetCharacter != null)
        {
            targetCharacter.GetComponent<CreaturePlayer>().CalculatePlayerHP(20f);
        }
    }
    /// <summary>
    /// �ִϸ��̼� ����
    /// </summary>
    public void ExitAttack()
    {
        animator.ResetTrigger("Try Attack");
    }
    #endregion


    public void Cancel()
    {
        Debug.Log("Fighter.Cancel()");

        animator.ResetTrigger("Try Attack");
    }
}
