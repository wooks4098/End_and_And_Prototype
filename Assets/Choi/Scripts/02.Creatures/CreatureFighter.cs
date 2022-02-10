using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    private Animator animator;
    private NavMeshAgent agent;

    private CreatureTargetFinder_Test finder;
        
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    /* ============== üũ�� bool Ÿ�� ============== */
    // ���� ���ΰ�
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }

    /* ============== ���� Ƚ�� ============== */
    [SerializeField] int attackCount = 0;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    // ���� �ӵ� - ��Ʈ�ѷ��κ��� �޾ƿ�
    private float attackSpeed;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        finder = GetComponent<CreatureTargetFinder_Test>();
    }

    private void OnEnable()
    {
        attackCount = 0;
    }

    public void StartAttackBehaviour()
    {
        Debug.Log("Fighter.StartAttackBehaviour()");
        GetComponent<CreatureActionScheduler>().StartAction(this);

        // Ÿ�� ã��
        finder.FindTarget();

        // ����
        Attack();
    }

    private void Attack()
    {
        // ���� ������ ǥ��
        isAttacking = true;

        // �ִϸ�����
        // ���� �ӵ�
        attackSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", attackSpeed);
        // ���� ����
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");        

        // ������Ʈ
        agent.isStopped = true;

        // Ÿ���� ������� ������
        if (finder.GetTestTarget() != null)
        {
            // �������� �ش�
            Debug.Log("AttackBehaviour()");
        }
    }


    #region call in attack animation 
    /// <summary>
    /// �ִϸ��̼� 
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;

        if (finder.GetTestTarget() != null)
        {
            finder.GetTestTarget().CalculatePlayerHP(20f);
        }

        if(!GetComponent<CreatureCaster>().GetIsCasting())
        {
            // ���� Ƚ�� ����
            attackCount++;
        }
    }

    /// <summary>
    /// �ִϸ��̼� ����
    /// </summary>
    public void ExitAttack()
    {
        animator.ResetTrigger("Try Attack");

        // ���� ���� �ƴ��� ǥ��
        isAttacking = false;
    }
    #endregion

    public void Cancel()
    {
        isAttacking = false;
    }
}
