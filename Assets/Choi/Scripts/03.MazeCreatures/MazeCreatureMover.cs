using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureMover : MonoBehaviour, ICreatureAction
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    // 컴포넌트
    private Animator animator;
    private NavMeshAgent agent;

    /* ============== 시간 ================ */
    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 2f;
    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== 코루틴 ================ */
    // 다음 패트롤을 기다리는 코루틴
    private Coroutine waitNextPatrolCoroutine;
    // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용
    private Coroutine timeLastPatrolCoroutine;

    /* ============== 체크용 bool 타입 ================ */
    [SerializeField] bool hasTarget = false; // 타겟유무

    /* ============== 타겟 ================ */
    // 실제 타겟
    [SerializeField] CreaturePlayer trackingTargetCharacter;
    public CreaturePlayer GetTargetCharacter() { return trackingTargetCharacter; }

    /* ============== 좌표/포지션 ================ */
    // 패트롤 할 자리
    [SerializeField] MazePatrolPath patrolPath;
    // 현재 index
    private int currentWaypointIndex;
    // 다음 포지션
    private Vector3 v3nextPosition;    



    #region DrawGizmos
    private void OnDrawGizmos()
    {
        // 다음 (목표) 위치
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(v3nextPosition, 3f);
    }

    #endregion
    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 5f;

        v3nextPosition = GetCurrentWaypoint();
    }
    private void OnDisable()
    {

    }

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        FindTrackingTargetCharacter();

        if (IsInTrackingRange())
        {
            StartTrackingBehaviour();
        }
    }
    
    /// <summary>
    /// 플레이어와의 거리 계산 (트래킹 범위)
    /// </summary>
    private bool IsInTrackingRange()
    {
        if (trackingTargetCharacter == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(trackingTargetCharacter.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // 비교한 값이 tracking 범위보다 적으면 true
        return distanceToPlayer < creature.GetTrackingRange();
    }

    /// <summary>
    /// StartMoveBehavour?
    /// </summary>
    public void StartPatrolBehaviour()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPath == null) return;

        // 에이전트 설정
        agent.updateRotation = true;
        agent.isStopped = false;
        // 에이전트 좌표 설정        
        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // 애니메이션
        animator.SetFloat("Speed", 0.1f);

        // 지정한 위치에 도착했는지
        if (AtWaypoint())
        {
            // 도착했으면 1. 애니메이션 Idle로
            animator.SetFloat("Speed", 0.0f);
            // 2. 멈춤
            agent.velocity = Vector3.zero;

            // 마지막 패트롤 시간 0으로 초기화
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol());
            }     
        }
    }

    /// <summary>
    /// 도착했는지 판별하는 메서드
    /// </summary>
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, v3nextPosition);
        return distanceToWaypoint <= 2.6f;
    }
    /// <summary>
    /// 패드롤 할 인덱스 순환 (-> 새로운 인덱스를 얻음)
    /// </summary>
    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }
    /// <summary>
    /// 새로운 인덱스를 토대로 다음 좌표를 얻는다
    /// nextPosition = GetCurrentWayPoint();
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    /// <summary>
    /// 다음 패트롤 좌표를 찾기까지 시간을 재는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitNextPatrol()
    {
        while (true)
        {
            timeForWaitingPatrol -= Time.deltaTime;

            yield return new WaitForFixedUpdate();

            if (timeForWaitingPatrol < 0f)
            {
                // 새로운 좌표를 지정하고
                CycleWaypoint(); // 인덱스 변경
                v3nextPosition = GetCurrentWaypoint(); // 새로운 인덱스를 토대로 다음좌표 설정
               
                break; // 빠져나간다
            }
        }
        // 시간을 2로
        timeForWaitingPatrol = 2f;
        // 코루틴 비우기
        waitNextPatrolCoroutine = null;
    }

    public void StartTrackingBehaviour()
    {
        Debug.Log("Mover.StartTrackingBehaviour()");
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        Tracking();
    }
    private void Tracking()
    {
        transform.LookAt(trackingTargetCharacter.transform);

        // 애니메이션
        animator.SetFloat("Speed", 0.6f);

        // 다음 목표 좌표를 플레이어로 설정
        v3nextPosition = trackingTargetCharacter.transform.position;

        // 다음 목표로 이동
        agent.destination = v3nextPosition;
        // 트래킹 속도로 전환
        agent.speed = creature.GetTrackingSpeed();
    }

    /// <summary>
    ///  쫓아갈 타겟 캐릭터 찾기
    /// </summary>
    private void FindTrackingTargetCharacter()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());

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
                agent.isStopped = false;

                trackingTargetCharacter = activeCollider.GetComponent<CreaturePlayer>();

                hasTarget = true;
            }
        }
    }


    public void Cancel()
    {
        Debug.Log("Mover.Cancel()");

        // 코루틴 진행중이면 강제로 코루틴 멈춤
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }

        // 시간 초기화
        timeForWaitingPatrol = 2f;

        // target을 비운다
        trackingTargetCharacter = null;

        // agent 초기화
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}
