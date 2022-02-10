using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureFighter : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    private Animator animator;
    private NavMeshAgent agent;
    private CreatureTargetFinder finder;

    // ũ���� ����
    [SerializeField] CreatureSO creature;

    /* ============== üũ�� bool Ÿ�� ============== */
    // ���� ���ΰ�
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }

    // Ÿ�� Ÿ��
    PlayerType targetType;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        finder = GetComponent<CreatureTargetFinder>();
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
        animator.SetTrigger("Try Attack");
        // ������Ʈ
        agent.isStopped = true;

        // Ÿ���� ������� ������
        if (finder.GetTarget() != null)
        {
            // �������� �ش�
            Debug.Log("AttackBehaviour()");
        }
    }

    #region call in attack animation 
    /// <summary>
    /// �ִϸ��̼ǿ��� ȣ��
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;
        /*
         * ������ ó��
         * GameManager.Instance.Damage(PlayerType.FirstPlayer, 20f);
         */

        // Ÿ���� ������� ������
        if (finder.GetTarget() != null)
        {
            // tag�� "Player1"�̸�
            if(finder.GetTarget().gameObject.tag == "Player1")
            {
                Debug.Log("Damage to Player1");
                // �������� �ִ� �Լ� ȣ��... 
                GameManager.Instance.PlayerDamage(PlayerType.FirstPlayer, creature.GetDamageValue());
            }
            // tag�� "Player2"�̸�
            else if (finder.GetTarget().gameObject.tag == "Player2")
            {
                Debug.Log("Damage to Player2");
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

        // ���� ���� �ƴ��� ǥ��
        isAttacking = false;
    }
    #endregion


    public void Cancel()
    {
        Debug.Log("Fighter.Cancel()");

        ExitAttack();
    }
}
