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
    private CreatureTargetFinder finder;

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

    // 타겟 포지션
    Transform targetPosition1;
    Transform targetPosition2;

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
        Gizmos.DrawWireSphere(v3nextPosition, 0.2f);
    }

    #endregion
    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 3f;

        v3nextPosition = GetCurrentWaypoint();
    }
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        finder = GetComponent<CreatureTargetFinder>();
    }

    private void Start()
    {
        targetPosition1 = GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer);
        targetPosition2 = GameManager.Instance.GetPlayerTrans(PlayerType.SecondPlayer);        
    }

    private void Update()
    {
        finder.FindTarget();

        if (finder.IsInTrackingRange() && !finder.IsInAttackRange())
        {
            StartTrackingBehaviour();
        }
    }

    /// <summary>
    /// StartMoveBehavour?
    /// </summary>
    public void StartPatrolBehaviour()
    {
        Debug.Log("Mover.StartPatrolBehaviour()");

        GetComponent<CreatureActionScheduler>().StartAction(this);

        // 패트롤
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

            // 코루틴 시작
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
        return distanceToWaypoint <= 0.1f;
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
        transform.LookAt(finder.GetTarget().transform);

        // 타겟이 비어있을 때만 코루틴 실행
        if (finder.GetTarget() == null)
        {
            if (timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }
        }

        // 애니메이션
        animator.SetFloat("Speed", 0.6f);

        // 다음 목표 좌표를 플레이어로 설정
        // v3nextPosition = finder.GetTarget().transform.position;
        if(finder.GetTarget().CompareTag("Player1"))
        {
            // 좌표
            v3nextPosition = GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer).position;
        }
        else if (finder.GetTarget().CompareTag("Player2"))
        {
            // 좌표
            v3nextPosition = GameManager.Instance.GetPlayerTrans(PlayerType.SecondPlayer).position;
        }

        // 다음 목표로 이동
        agent.destination = v3nextPosition;

        // 트래킹 속도로 전환
        agent.speed = creature.GetTrackingSpeed();
    }

    private IEnumerator TimeLastPatrol()
    {
        while (true)
        {
            // 임시 타겟이 생기는 순간 루프를 빠져나감
            if (finder.GetTarget() != null) break;

            // 마지막으로 패트롤한지
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 10초를 초과하면
            if (timeSinceLastPatrol > 10f)
            {
                // 타겟을 찾아본다
                finder.FindTarget();

                // 임시 타겟이 없으면
                if (finder.GetTarget() == null)
                {
                    // 빠져나간다
                    break;
                }
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
        timeForWaitingPatrol = 3f;

        // target을 비운다
        // trackingTargetCharacter = null;

        // agent 초기화
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}
