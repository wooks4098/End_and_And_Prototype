using System;
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

    /* ============== ���� Ƚ�� ============== */
    [SerializeField] int attackCount;
    public int GetAttackCount() { return attackCount; }

    /* ============== Ÿ�� ================ */
    // ���� Ÿ��
    [SerializeField] CreaturePlayer targetCharacter;
    // [SerializeField] List<CreaturePlayer> targetCharacters;

    // ���� �ӵ� - ��Ʈ�ѷ��κ��� �޾ƿ�
    private float attackSpeed;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        attackCount = 0;
    }

    public void StartAttackBehaviour()
    {
        Debug.Log("Fighter.StartAttackBehaviour()");
        GetComponent<CreatureActionScheduler>().StartAction(this);

        Attack();
    }

    private void Attack()
    {
        isAttacking = true;

        // �ִϸ�����
        // ���� �ӵ�
        attackSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", attackSpeed);
        // ���� ����
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");        

        // Ÿ���� ������� ������
        if (targetCharacter != null)
        {
            // �������� �ش�
            Debug.Log("AttackBehaviour()");
        }
    }

    /// <summary>
    /// ���� Ƚ�� ���
    /// </summary>
    private void CalculateAttackCount()
    {
        Debug.Log("CalculateAttackCount");

        // ����Ƚ���� 2���� �� ��
        if(attackCount <= 2)
        {
            // ����Ƚ�� +1 ����
            attackCount = attackCount + 1;
        }
        // ����Ƚ���� 2���� ũ��
        else if(attackCount > 2)
        {
            // ĳ���� ���� ����
            GetComponent<CreatureController>().CanCasting = true;
            // ����Ƚ�� 0���� �ʱ�ȭ
            attackCount = 0;
        }
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
        // Ÿ�� ã��
        FindTargetsForAttack();

        // if (targetCharacter.GetIsDead()) return;

        if (targetCharacter != null)
        {
            targetCharacter.GetComponent<CreaturePlayer>().CalculatePlayerHP(20f);
        }

        // ���� Ƚ�� ���
        CalculateAttackCount();
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
