using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    //CreatureMovement patroller;
    CreaturePatroller patroller;
    CreatureTracker tracker;    
    CreatureCaster caster;
    CreatureFighter fighter;

    [SerializeField] bool hasTarget;
    [SerializeField] bool isCasting = false;
    [SerializeField] bool canAttack = false;


    [SerializeField] Creature creature;

    [SerializeField] CreaturePlayer tempTarget;
    [SerializeField] CreaturePlayer targetCharacter;

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;
    
    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // 패트롤 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, creature.patrolRange);

        // 인식 시야 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, creature.trackingRange);

        // 공격 거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, creature.attackRange);

        // 다음 (목표) 위치
        // Gizmos.color = Color.cyan;
        // Gizmos.DrawWireSphere(targetPosition.position, 3);
    }

    #endregion

    private void Awake()
    {
        patroller = GetComponent<CreaturePatroller>();
        tracker = GetComponent<CreatureTracker>();
        caster = GetComponent<CreatureCaster>();
        fighter = GetComponent<CreatureFighter>();
    }

    private void Start()
    {
        //createPosition = CreaturePool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;
    }

    private void Update()
    {
        if (hasTarget)
        {
            // 공격 범위에 들어오면
            if (IsInAttackRange())
            {
                if (!canAttack)
                {
                    CastBehaviour();
                }
                else
                {
                    AttackBehaviour();
                }
            }
            // 트래킹 범위에 들어오면
            else if (IsInTrackingRange())
            {
                if (!isCasting)
                {
                    TrackBehaviour();
                }
                else if (canAttack)
                {
                    AttackBehaviour();
                }
            }
        }
        else
        {
            PatrolBehaviour();
        }
    }


    /// <summary>
    /// 플레이어와 거리 계산 (공격 범위)
    /// </summary>
    private bool IsInAttackRange()
    {
        if (targetCharacter == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(targetCharacter.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 attack 범위보다 적으면 true
        return distanceToPlayer < creature.attackRange;
    }

    /// <summary>
    /// 플레이어와의 거리 계산 (트래킹 범위)
    /// </summary>
    private bool IsInTrackingRange()
    {
        if (targetCharacter == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(targetCharacter.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // 비교한 값이 tracking 범위보다 적으면 true
        return distanceToPlayer < creature.trackingRange;
    }


    private void PatrolBehaviour()
    {
        patroller.StartPatrolBehaviour(createPosition);
    }

    private void TrackBehaviour()
    {
        tracker.StartTrackingBehaviour(tempTarget, targetCharacter);
    }

    private void CastBehaviour()
    {
        caster.StartSpellCastBehaviour();
    }

    private void AttackBehaviour()
    {
        fighter.StartAttackBehaviour();
    }
}
