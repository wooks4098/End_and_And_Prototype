using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 크리쳐의 움직임을 관리하는 클래스
/// </summary>
public class CreatureMovement : MonoBehaviour
{
    [SerializeField] bool isActive;
    [SerializeField] bool hasTarget;

    [SerializeField] Creature creature;
    private NavMeshAgent agent;

    [SerializeField] CreaturePlayer player;

    [SerializeField] Vector3 createPosition;
    private Vector3 currentPosition;
    private Vector3 nextPosition;

    // 적을 감지할 시야의 기준점 (영역 내에서 감지)
    public Transform eyeTransform;

    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    // 마지막으로 플레이어를 본 시간
    [SerializeField] float timeSinceLastSawPlayer = 0f;


    #region Gizmos

    private void OnDrawGizmos()
    {
        // 패트롤 범위
        Gizmos.color = Color.blue;        
        Gizmos.DrawWireSphere(createPosition, creature.patrolRange);

        // 인식 시야 범위
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, creature.trackingRange);

        // 공격 거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, creature.attackDistance);

        // 공격 거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(nextPosition, 5);
    }

    #endregion

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        currentPosition = createPosition;
        nextPosition = currentPosition;

        // agent.stoppingDistance를 이용하면 어느 정도 거리에서 멈출지 설정할 수 있다.
        // agent.stoppingDistance = creature.attackDistance;

        //PatrolBehaviour();
    }

    private void Update()
    {
        if (!isActive) return;

        if(IsInAttackRange())
        {
            AttackBehaviour();
        }
        else if (IsInTrackingRange())
        {
            TrackingBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        // UpdateTimers();
    }

    /// <summary>
    /// 패트롤 행동
    /// </summary>
    private void PatrolBehaviour()
    {
        // 지정한 도착했는지
        if(IsArrive())
        {
            UpdatePath();
        }
        else
        {
            // 마지막으로 패트롤한지
            timeSinceLastPatrol += Time.deltaTime;
            // 3초를 초과하면
            if(timeSinceLastPatrol > 7f)
            {
                // 새로운 좌표를 지정하고
                UpdatePath();

                // 다시 시간을 0으로
                timeSinceLastPatrol = 0f;
            }
        }
    }

    /// <summary>
    /// 좌표를 갱신하는 메서드
    /// </summary>
    private void UpdatePath()
    {
        // 랜덤 X, Z 좌표 생성 - CreatePosition을 중심으로
        float randomX = UnityEngine.Random.Range(0, createPosition.x);
        float randomZ = UnityEngine.Random.Range(0, createPosition.z);
        // 움직일 포지션 지정
        Vector3 movePosition = new Vector3(randomX, createPosition.y, randomZ);

        // 다음 목표 좌표를 movePosition으로 할당
        nextPosition = movePosition;
        // 자동 멈춤 거리가 0으로 지정되어있지 않은 경우
        if(agent.stoppingDistance != 0f)
        {
            // 0으로 초기화 해준다.
            agent.stoppingDistance = 0f;
        }
        // 움직일 좌표를 nextPosition으로 할당
        agent.destination = nextPosition;
        agent.speed = creature.patrolSpeed;
    }

    /// <summary>
    /// 도착했는지 판별하는 메서드
    /// </summary>
    private bool IsArrive()
    {
        // 다음 포지션과 크리쳐의 거리 계산
        float distanceToWaypoint = Vector3.Distance(transform.position, nextPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint < 1f;
    }

    private void TrackingBehaviour()
    {
        // 다음 목표 좌표를 플레이어로 설정
        nextPosition = player.transform.position;

        // agent.stoppingDistance를 이용하면 어느 정도 거리에서 멈출지 설정할 수 있다.
        agent.stoppingDistance = creature.attackDistance;
        // 다음 목표로 이동
        agent.destination = nextPosition;
        // 트래킹 속도로 전환
        agent.speed = creature.trackingSpeed;
    }

    /// <summary>
    /// 공격 행동
    /// </summary>
    private void AttackBehaviour()
    {
        // 플레이어를 바라보고
        transform.LookAt(player.transform);
        // 공격한다
        Debug.Log("AttackBehaviour()");
    }

    /// <summary>
    /// 플레이어와 거리 계산 (공격 범위)
    /// </summary>
    private bool IsInAttackRange()
    {
        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 attack 범위보다 적으면 true
        return distanceToPlayer < creature.attackRange;
    }

    /// <summary>
    /// 플레이어와의 거리 계산 (트래킹 범위)
    /// </summary>
    private bool IsInTrackingRange()
    {
        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 tracking 범위보다 적으면 true
        return distanceToPlayer < creature.trackingRange;
    }
}
