using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureFighter : MonoBehaviour, ICreatureAction
{
    // 컴포넌트
    Animator animator;
    NavMeshAgent agent;

    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    /* ============== 체크용 bool 타입 ============== */
    // 공격 중인가
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }

    /* ============== 타겟 ================ */
    // 실제 타겟
    [SerializeField] PlayerController targetCharacter;
    // 타겟 타입
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
        // 공격 중임을 표시
        isAttacking = true;

        // 애니메이터
        animator.SetTrigger("Try Attack");
        // 에이전트
        agent.isStopped = true;

        // 타겟을 찾는다
        FindTargetsForAttack();

        // 타겟이 비어있지 않으면
        if (targetCharacter != null)
        {
            // 데미지를 준다
            Debug.Log("AttackBehaviour()");
        }

        // 공격할 수 없다고 체크
        GetComponent<MazeCreatureController>().CanAttack = false;
    }

    /// <summary>
    /// 공격할 타겟 캐릭터 찾기
    /// </summary>
    private void FindTargetsForAttack()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetAttackRange());

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("뭔가 찾았습니다!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. 플레이어 관련 컴포넌트를 가지고 있고 2. 죽지않았고 3. 활성화 되어있는 것
            if (activeCollider.GetComponent<PlayerController>() != null
                && activeCollider.GetComponent<PlayerController>().GetPlayerState() != PlayerState.Crawl
                && activeCollider.gameObject.activeSelf)
            {
                // 타겟 지정
                targetCharacter = activeCollider.GetComponent<PlayerController>();
            }
        }
    }

    #region call in attack animation 
    /// <summary>
    /// 애니메이션 
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;
        /*
         * 데미지 처리
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
    /// 애니메이션 리셋
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
