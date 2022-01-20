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
    }



    /// <summary>
    /// 플레이어와 거리 계산 (공격 범위)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (GetComponent<CreatureMover>().GetTargetCharacter() == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(GetComponent<CreatureMover>().GetTargetCharacter().transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 attack 범위보다 적으면 true
        return distanceToPlayer < creature.GetAttackRange();
    }

    public void Cancel()
    {
        // StopAttack();
        // target = null;
        GetComponent<CreatureTracker>().Cancel();
    }




}
