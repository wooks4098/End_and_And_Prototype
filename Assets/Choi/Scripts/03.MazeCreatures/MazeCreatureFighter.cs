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
    [SerializeField] PlayerController targetCharacter;
    // Ÿ�� Ÿ��
    PlayerType targetType;


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
            if (activeCollider.GetComponent<PlayerController>() != null
                && activeCollider.GetComponent<PlayerController>().GetPlayerState() != PlayerState.Crawl
                && activeCollider.gameObject.activeSelf)
            {
                // Ÿ�� ����
                targetCharacter = activeCollider.GetComponent<PlayerController>();
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
        /*
         * ������ ó��
         * GameManager.Instance.Damage(PlayerType.FirstPlayer, 20f);
         */

        if (targetCharacter != null)
        {
            if(targetCharacter.gameObject.tag == "Player1")
            {
                GameManager.Instance.PlayerDamage(PlayerType.FirstPlayer, creature.GetDamageValue());
            }
            else if(targetCharacter.gameObject.tag == "Player2")
            {
                GameManager.Instance.PlayerDamage(PlayerType.SecondPlayer, creature.GetDamageValue());
            }
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
