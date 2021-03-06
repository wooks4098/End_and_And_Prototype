using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureMover : MonoBehaviour, ICreatureAction
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    // 컴포넌트
    private Animator animator;
    private NavMeshAgent agent;

    private CreatureTargetFinder finder;

    /* ============== 시간 ================ */
    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 5f;
    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== 코루틴 ================ */
    // 다음 패트롤을 기다리는 코루틴
    private Coroutine waitNextPatrolCoroutine;
    // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용
    private Coroutine timeLastPatrolCoroutine;

    /* ============== 체크용 bool 타입 ================ */
    [SerializeField] bool hasTarget = false; // 타겟유무

    // 다음 포지션
    private Vector3 v3nextPosition;
    // navmeshHit
    private NavMeshHit hit;

    // 생성 좌표
    [SerializeField] Transform createPosition;


    private void OnDrawGizmos()
    {
        // 다음 (목표) 위치
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(v3nextPosition, 3);
    }

    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 5f;

        createPosition = GetComponent<CreatureController>().GetCreatePosition();
        v3nextPosition = createPosition.position;
    }

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        finder = GetComponent<CreatureTargetFinder>();
    }
    private void Update()
    {
        if (GetComponent<CreatureCaster>().GetIsCasting()) return;

        finder.FindTarget();

        if(finder.IsInTrackingRange() && !finder.IsInAttackRange())
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

        Patrol();
    }

    private void Patrol()
    {
        agent.updateRotation = true;
        agent.isStopped = false;

        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // 애니메이션
        animator.SetFloat("Speed", 0.1f);

        // 지정한 위치에 도착했는지
        if (IsArrive())
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

    public void StartTrackingBehaviour()
    {
        Debug.Log("Mover.StartTrackingBehaviour()");
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        Tracking();
    }
    private void Tracking()
    {
        transform.LookAt(finder.GetTarget().transform);

        // 임시 타겟이 비어있을 때만 코루틴 실행
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
        v3nextPosition = finder.GetTarget().transform.position;

        // 다음 목표로 이동
        agent.destination = v3nextPosition;
        // 트래킹 속도로 전환
        agent.speed = creature.GetTrackingSpeed();
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
                UpdatePath();
                // 빠져나간다
                break;
            }
        }
        // 시간을 5로
        timeForWaitingPatrol = 5f;
        // 코루틴 비우기
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// 도착했는지 판별하는 메서드
    /// </summary>
    private bool IsArrive()
    {
        // 다음 포지션과 크리쳐의 거리 계산
        float distanceToWaypoint = Vector3.Distance(transform.position, v3nextPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }

    /// <summary>
    /// 패트롤 시간이 얼마나 지났는지를 재는 코루틴
    /// 임시타겟 (=tempTarget)이 없을 때 실행
    /// </summary>
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
                    // 1. 타겟을 없음으로 표시
                    // hasTarget = false;

                    // 2. 실제 타겟을 비우고
                    // finder.GetTarget() = null;

                    // 3. 새로운 좌표를 지정
                    UpdatePath();

                    // 빠져나간다
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 좌표를 갱신하는 메서드
    /// </summary>
    public void UpdatePath()
    {
        // 랜덤 X, Z 좌표 생성 - CreatePosition을 중심으로
        // createPosition - creature.patrolRange => (생성 포지션 - 크리쳐 패트롤 범위)        
        float randomX = UnityEngine.Random.Range(createPosition.position.x - creature.GetPatrolRange(), createPosition.position.x + creature.GetPatrolRange());
        float randomZ = UnityEngine.Random.Range(createPosition.position.z - creature.GetPatrolRange(), createPosition.position.z + creature.GetPatrolRange());

        // 다음으로 움직일 포지션 지정
        v3nextPosition = new Vector3(randomX, transform.position.y, randomZ);

        // 좌표를 미리 계산해보고 hit로 반환
        // ((주의)) 매개변수 maxDistance 부분이 작아질 수록 연산량이 많아짐 -> 스택 오버플로우!! 
        // 이를 방지하기 위해 NavMash를 좀 두껍게 처리함... (ㅠㅠ)
        NavMesh.SamplePosition(v3nextPosition, out hit, 10f, 1);

        // 디버그 찍었을 때 bake 된 영역이 아니면 x,y,z 좌표 전부 Infinity가 뜸!!!
        // Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);

        // bake 된 영역 바깥이면 
        if (hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // 좌표를 새로 갱신 
            UpdatePath();
        }

        v3nextPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // 회전
        // 회전 업데이트 멈춤
        agent.updateRotation = false;
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // 타겟이 있으면 타겟 바라보기 
        // if (targetCharacter != null)
        // {
        //     transform.LookAt(targetCharacter.transform);
        // }

        // agent.updateRotation = true;
    }
    

    public void Cancel()
    {
        Debug.Log("Mover.Cancel()");

        // target을 비운다
        // tempTarget = null;
        // trackingTargetCharacter = null;

        // agent 초기화
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // 코루틴 진행중이면 강제로 코루틴 멈춤
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
            // 시간 초기화
            timeForWaitingPatrol = 5f;
            // 코루틴 비우기
            waitNextPatrolCoroutine = null;
        }
        if (timeLastPatrolCoroutine != null)
        {
            StopCoroutine(timeLastPatrolCoroutine);
            // 시간 초기화
            timeSinceLastPatrol = 0f;
            // 코루틴 비우기
            timeLastPatrolCoroutine = null;
        }
    }
}