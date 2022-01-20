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


    /* ============== 타겟 ================ */
    // 임시 타겟
    [SerializeField] CreaturePlayer tempTarget;
    // 실제 타겟
    [SerializeField] CreaturePlayer targetCharacter;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public void StartAttackBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);

        // Caster 컴포넌트가 비어있지 않으면
        if(GetComponent<CreatureCaster>() != null)
        {
            // 
        }

        Attack();        
    }

    private void Attack()
    {
        isAttacking = true;
        if (targetCharacter == null) return;
        
        // 플레이어를 바라보고
        transform.LookAt(targetCharacter.transform);
        // 멈춘다
        agent.velocity = Vector3.zero;
        agent.isStopped = false;

        // 애니메이터
        animator.SetTrigger("Run Attack");
        // animator.ResetTrigger("Prepare Attack");

        if(targetCharacter != null)
        {
            // 공격한다
            Debug.Log("AttackBehaviour()");
        }
    }



    public void Cancel()
    {

    }
}
