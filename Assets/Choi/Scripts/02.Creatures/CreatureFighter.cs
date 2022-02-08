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
    [SerializeField] int attackCount;
    public int GetAttackCount() { return attackCount; }

    /* ============== 타겟 ================ */
    // 실제 타겟
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

        // Caster 컴포넌트가 비어있지 않으면
        if(GetComponent<CreatureCaster>() != null)
        {
            // 임시 예외처리용
        }

        Attack();        
    }

    private void Attack()
    {
        isAttacking = true;
        if (targetCharacters == null) return;
        
        // 애니메이터
        animator.SetTrigger("Try Attack");
        // animator.ResetTrigger("Prepare Attack");

        SetAttackTarget();

        // 타겟이 비어있지 않으면
        if (targetCharacters != null)
        {
            // 데미지를 준다
            Debug.Log("AttackBehaviour()");

            // 공격 카운트 +1
            CalculateAttackCount();
        }

        // 공격할 수 없다고 체크
        GetComponent<CreatureController>().CanAttack = false;
    }

    private void CalculateAttackCount()
    {
        attackCount ++;
    }

    private void SetAttackTarget()
    {
        // caster 컴포넌트가 있으면
        if(GetComponent<CreatureCaster>() != null)
        {
            // caster 컴포넌트로부터 타겟을 가져온다
            targetCharacters = GetComponent<CreatureCaster>().GetTargetCharacter();
        }
        // caster 컴포넌트가 없으면
        else
        {
            // 범위 안에 있는 걸 가져온다~
            FindTargetsForAttack();
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
                targetCharacters.Add(activeCollider.GetComponent<CreaturePlayer>());
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
    /// 애니메이션 리셋
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
