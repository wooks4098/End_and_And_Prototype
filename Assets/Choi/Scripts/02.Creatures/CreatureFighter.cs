using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    // 컴포넌트
    private Animator animator;
    private NavMeshAgent agent;

    private CreatureTargetFinder_Test finder;
        
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    /* ============== 체크용 bool 타입 ============== */
    // 공격 중인가
    [SerializeField] bool isAttacking = false;
    public bool GetIsAttacking() { return isAttacking; }

    /* ============== 공격 횟수 ============== */
    [SerializeField] int attackCount = 0;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    // 공격 속도 - 컨트롤러로부터 받아옴
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
        // 공격 속도
        attackSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", attackSpeed);
        // 공격 실행
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");        

        // 에이전트
        agent.isStopped = true;

        // 타겟이 비어있지 않으면
        if (finder.GetTestTarget() != null)
        {
            // 데미지를 준다
            Debug.Log("AttackBehaviour()");
        }
    }


    #region call in attack animation 
    /// <summary>
    /// 애니메이션 
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
            // 공격 횟수 증가
            attackCount++;
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
        isAttacking = false;
    }
}
