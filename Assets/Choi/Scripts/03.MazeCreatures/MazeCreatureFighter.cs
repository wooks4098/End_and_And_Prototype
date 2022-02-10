using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureFighter : MonoBehaviour, ICreatureAction
{
    // 컴포넌트
    private Animator animator;
    private NavMeshAgent agent;
    private CreatureTargetFinder finder;

    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    /* ============== 체크용 bool 타입 ============== */
    // 공격 중인가
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }

    // 타겟 타입
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

        // 타겟 찾기
        finder.FindTarget();

        // 공격
        Attack();
    }
    private void Attack()
    {
        // 공격 중임을 표시
        isAttacking = true;

        // 애니메이터
        animator.SetTrigger("Try Attack");
        // 에이전트
        agent.isStopped = true;

        // 타겟이 비어있지 않으면
        if (finder.GetTarget() != null)
        {
            // 데미지를 준다
            Debug.Log("AttackBehaviour()");
        }
    }

    #region call in attack animation 
    /// <summary>
    /// 애니메이션에서 호출
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;
        /*
         * 데미지 처리
         * GameManager.Instance.Damage(PlayerType.FirstPlayer, 20f);
         */

        // 타겟이 비어있지 않으면
        if (finder.GetTarget() != null)
        {
            // tag가 "Player1"이면
            if(finder.GetTarget().gameObject.tag == "Player1")
            {
                Debug.Log("Damage to Player1");
                // 데미지를 주는 함수 호출... 
                GameManager.Instance.PlayerDamage(PlayerType.FirstPlayer, creature.GetDamageValue());
            }
            // tag가 "Player2"이면
            else if (finder.GetTarget().gameObject.tag == "Player2")
            {
                Debug.Log("Damage to Player2");
                GameManager.Instance.PlayerDamage(PlayerType.SecondPlayer, creature.GetDamageValue());
            }
        }
    }
    /// <summary>
    /// 애니메이션 리셋
    /// </summary>
    public void ExitAttack()
    {
        animator.ResetTrigger("Try Attack");

        // 공격 중이 아님을 표시
        isAttacking = false;
    }
    #endregion


    public void Cancel()
    {
        Debug.Log("Fighter.Cancel()");

        ExitAttack();
    }
}
