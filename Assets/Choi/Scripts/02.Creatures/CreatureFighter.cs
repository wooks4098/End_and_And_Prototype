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
    [SerializeField] List<CreaturePlayer> targetCharacters;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }
    private void Start()
    {
        targetCharacters = new List<CreaturePlayer>();
    }

    private void OnEnable()
    {
        attackCount = 0;
    }

    public void StartAttackBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);

        // Caster ������Ʈ�� ������� ������
        if(GetComponent<CreatureCaster>() != null)
        {
            // �ӽ� ����ó����
        }

        Attack();        
    }

    private void Attack()
    {
        isAttacking = true;
        if (targetCharacters == null) return;
        
        // �ִϸ�����
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");

        SetAttackTarget();

        // Ÿ���� ������� ������
        if (targetCharacters != null)
        {
            // �������� �ش�
            Debug.Log("AttackBehaviour()");

            // ���� ī��Ʈ +1
            CalculateAttackCount();
        }

        // ������ �� ���ٰ� üũ
        GetComponent<CreatureController>().CanAttack = false;
    }

    private void CalculateAttackCount()
    {
        attackCount ++;
    }

    private void SetAttackTarget()
    {
        // caster ������Ʈ�� ������
        if(GetComponent<CreatureCaster>() != null)
        {
            // caster ������Ʈ�κ��� Ÿ���� �����´�
            targetCharacters = GetComponent<CreatureCaster>().GetTargetCharacter();
        }
        // caster ������Ʈ�� ������
        else
        {
            // ���� �ȿ� �ִ� �� �����´�~
            FindTargetsForAttack();
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
                targetCharacters.Add(activeCollider.GetComponent<CreaturePlayer>());
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

        if (targetCharacters[0] != null)
        {
            targetCharacters[0].GetComponent<CreaturePlayer>().CalculatePlayerHP(20f);
        }
        // if (targetCharacters[1] != null)
        // {
        //     targetCharacters[1].GetComponent<CreaturePlayer>().CalculatePlayerHP(20f);
        // }
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
        
    }
}
