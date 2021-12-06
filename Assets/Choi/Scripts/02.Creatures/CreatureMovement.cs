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

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

    // 적을 감지할 시야의 기준점 (영역 내에서 감지)
    public Transform eyeTransform;

    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 5f;

    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    // 마지막으로 플레이어를 본 시간
    private float timeSinceLastSawPlayer = 0f;

    private Coroutine waitNextPatrolCoroutine;
    private Coroutine timeLastPatrolCoroutine;

    // path
    NavMeshPath path;
    private NavMeshHit hit;

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
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }
    private void Start()
    {
        targetPosition = createPosition.position;

        // agent.stoppingDistance를 이용하면 어느 정도 거리에서 멈출지 설정할 수 있다.
        // agent.stoppingDistance = creature.attackDistance;

        timeForWaitingPatrol = 5f;
    }

    private void Update()
    {
        // 공격 범위에 들어오면
        if (IsInAttackRange())
        {
            creature.state = CreatureState.Attack;
        }
        // 트래킹 범위에 들어오면
        else if (IsInTrackingRange())
        {
            creature.state = CreatureState.Tracking;
        }
        else
        {
            creature.state = CreatureState.Patrol;
        }

        DecisionBehaviour(creature.state);
    }

    private void DecisionBehaviour(CreatureState _state)
    {
        switch (_state)
        {
            case CreatureState.Patrol:
                PatrolBehaviour();
                break;
            case CreatureState.Tracking:
                TrackingBehaviour();
                break;
            case CreatureState.Attack:
                AttackBehaviour();
                break;
            default:
                PatrolBehaviour();
                break;
        }
    }

    /// <summary>
    /// 패트롤 행동
    /// </summary>
    private void PatrolBehaviour()
    {
        // 지정한 도착했는지
        if (IsArrive())
        {
            // 마지막 패트롤 시간 0으로 초기화
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol());
            }
        }
    }

    private IEnumerator TimeLastPatrol()
    {
        while (true)
        {
            // 마지막으로 패트롤한지
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 3초를 초과하면
            if (timeSinceLastPatrol > 10f)
            {
                // 새로운 좌표를 지정하고
                UpdatePath();
                // 빠져나간다
                break;
            }
        }

        // 다시 시간을 0으로
        timeSinceLastPatrol = 0f;
        // 코루틴 비우기
        timeLastPatrolCoroutine = null;
    }

    private IEnumerator WaitNextPatrol()
    {
        while (true)
        {
            timeForWaitingPatrol -= Time.deltaTime;

            yield return new WaitForFixedUpdate();

            if (timeForWaitingPatrol < 0f)
            {
                // 새로운 좌표를 지정하고
                UpdatePath();
                // 빠져나간다
                break;
            }
        }
        // 시간을 5로
        timeForWaitingPatrol = 1f;
        // 코루틴 비우기
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// 좌표를 갱신하는 메서드
    /// </summary>
    private void UpdatePath()
    {

        // 랜덤 X, Z 좌표 생성 - CreatePosition을 중심으로
        // createPosition - creature.patrolRange => (생성 포지션 - 크리쳐 패트롤 범위)        
        float randomX = UnityEngine.Random.Range(createPosition.position.x - creature.patrolRange, createPosition.position.x + creature.patrolRange);
        float randomZ = UnityEngine.Random.Range(createPosition.position.z - creature.patrolRange, createPosition.position.z + creature.patrolRange);

        // 다음으로 움직일 포지션 지정
        targetPosition = new Vector3(randomX, 2.5f, randomZ);

        // 좌표를 미리 계산해보고 hit로 반환
        // ((주의)) 매개변수 maxDistance 부분이 작아질 수록 연산량이 많아짐 -> 스택 오버플로우!! 
        // 이를 방지하기 위해 NavMash를 좀 두껍게 처리함... (ㅠㅠ)
        NavMesh.SamplePosition(targetPosition, out hit, 5f, 1);

        // 디버그 찍었을 때 bake 된 영역이 아니면 x,y,z 좌표 전부 Infinity가 뜸!!!
        Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);
        // bake 된 영역 바깥이면 
        if(hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // 좌표를 새로 갱신 
            UpdatePath();
        }

        targetPosition = hit.position;
        Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;
    }
    private void TrackingBehaviour()
    {
        // 다음 목표 좌표를 플레이어로 설정
        targetPosition = player.transform.position;

        // 다음 목표로 이동
        agent.destination = targetPosition;
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
        agent.velocity = Vector3.zero;

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
        // Debug.Log(distanceToPlayer);

        // 비교한 값이 tracking 범위보다 적으면 true
        return distanceToPlayer < creature.trackingRange;
    }

    /// <summary>
    /// 도착했는지 판별하는 메서드
    /// </summary>
    private bool IsArrive()
    {
        // 다음 포지션과 크리쳐의 거리 계산
        float distanceToWaypoint = Vector3.Distance(transform.position, targetPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }


    public bool IsAgentOnNavMesh(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);

        if (!agent.pathPending)
        {
            return false;
        }

        return true;
    }
}