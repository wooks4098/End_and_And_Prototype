using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
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

    /* ============== 공격 횟수 ============== */
    [SerializeField] int attackCount = 0;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    /* ============== 타겟 ================ */
    // 실제 타겟
    [SerializeField] CreaturePlayer targetCharacter;
    // [SerializeField] List<CreaturePlayer> targetCharacters;

    // 공격 속도 - 컨트롤러로부터 받아옴
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

        // 타겟 찾기
        FindTargetsForAttack();

        // 공격
        Attack();
    }

    private void Attack()
    {
        isAttacking = true;

        // 애니메이터
        // 공격 속도
        attackSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", attackSpeed);
        // 공격 실행
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");        

        // 타겟이 비어있지 않으면
        if (targetCharacter != null)
        {
            // 데미지를 준다
            Debug.Log("AttackBehaviour()");
        }
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
            if (activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {
                // 타겟 지정
                targetCharacter = activeCollider.GetComponent<CreaturePlayer>();
            }
        }

        // 죽었을 때를 대비한 예외처리
        if(targetCharacter != null && targetCharacter.GetIsDead())
        {
            // 죽었으면 null 처리
            targetCharacter = null;
        }
    }


    #region call in attack animation 
    /// <summary>
    /// 애니메이션 
    /// </summary>
    public void BiteTest()
    {
        // if (targetCharacter.GetIsDead()) return;

        if (targetCharacter != null)
        {
            targetCharacter.GetComponent<CreaturePlayer>().CalculatePlayerHP(20f);
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
