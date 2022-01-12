using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 크리쳐의 움직임을 관리하는 클래스
/// </summary>
public class CreatureMovement : MonoBehaviour, ICreatureAction
{
    [SerializeField] Animator animator;

    [SerializeField] bool isActive;
    [SerializeField] bool hasTarget;
    // [SerializeField] bool isAttacking;

    [SerializeField] Creature creature;
    private NavMeshAgent agent;

    [SerializeField] CreaturePlayer targetCharacter; // 실제 타겟
    [SerializeField] CreaturePlayer tempTarget; // 임시로 저장할 타겟

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

    // 적을 감지할 시야의 기준점 (영역 내에서 감지)
    // public Transform eyeTransform;

    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 5f;

    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    // 마지막으로 플레이어를 본 시간
    private float timeSinceLastSawPlayer = 0f;

    private Coroutine waitNextPatrolCoroutine; // 다음 패트롤을 기다리는 코루틴
    private Coroutine timeLastPatrolCoroutine; // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용

    // path
    // NavMeshPath path;
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

    #region OnEnable, OnDisable

    private void OnEnable()
    {
        if(!agent.enabled)
        {
            agent.enabled = true;
        }

        //createPosition = CreaturePool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;

        creature.state = CreatureState.Patrol;
        timeForWaitingPatrol = 5f;

        //currentPosition = 

        targetPosition = createPosition.position;

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;
    }

    private void OnDisable()
    {
        
    }

    #endregion


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // path = new NavMeshPath();
    }
    private void Start()
    {
        // agent.stoppingDistance를 이용하면 어느 정도 거리에서 멈출지 설정할 수 있다.
        // agent.stoppingDistance = creature.attackDistance;

        timeForWaitingPatrol = 5f;
    }

    private void Update()
    {
        FindTargetCharacter();

        if (hasTarget)
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
        }
        else
        {
            creature.state = CreatureState.Patrol;
        }

        DecisionBehaviour(creature.state);
    }

    /// <summary>
    ///  타겟 캐릭터 설정
    /// </summary>
    private void FindTargetCharacter()
    {
        // Cancel();

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.trackingRange);

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("뭔가 찾았습니다!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. 플레이어 관련 컴포넌트를 가지고 있고 2. 죽지않았고 3. 활성화 되어있는 것
            if(activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {

                // 만약 임시 타겟이 비어있지 않고 (-> 임시 타겟이 비어있지 않을 때만 비교)
                // 임시 타겟의 점수와 새롭게 비교하는 타겟의 점수를 비교
                if (tempTarget != null
                    && tempTarget.score > activeCollider.gameObject.GetComponent<CreaturePlayer>().score)
                {
                    // 임시 타겟 쪽의 점수가 크면 계속 (continue)
                    continue;
                }
                else
                {
                    // 임시타겟 지정
                    tempTarget = activeCollider.gameObject.GetComponent<CreaturePlayer>();

                    // 애니메이션 멈춤
                    animator.SetBool("Attack", false);                    
                }

                // 타겟 지정
                targetCharacter = tempTarget;
                agent.isStopped = false;

                hasTarget = true;                
            }

            else
            {
                // 임시타겟을 비운다
                tempTarget = null;
            }
        }        
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
        agent.updateRotation = true;

        // 애니메이션
        animator.SetFloat("Speed", 0.1f);

        // 지정한 도착했는지
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

    /// <summary>
    /// 패트롤 시간이 얼마나 지났는지를 재는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeLastPatrol()
    {
        while (true)
        {
            // 임시 타겟이 생기는 순간 루프를 빠져나감
            if (tempTarget != null) break;

            // 마지막으로 패트롤한지
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 10초를 초과하면
            if (timeSinceLastPatrol > 10f)
            {
                // 타겟을 찾아본다
                // FindTargetCharacter();

                // 임시 타겟이 없으면
                if(tempTarget == null)
                {
                    // 1. 타겟을 없음으로 표시
                    hasTarget = false;

                    // 2. 실제 타겟을 비우고
                    targetCharacter = null;

                    // 3. 새로운 좌표를 지정
                    UpdatePath();

                    // 빠져나간다
                    break;
                }
            }
        }

        // 다시 시간을 0으로
        timeSinceLastPatrol = 0f;
        // 코루틴 비우기
        timeLastPatrolCoroutine = null;
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
        timeForWaitingPatrol = 2f;
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
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        // 좌표를 미리 계산해보고 hit로 반환
        // ((주의)) 매개변수 maxDistance 부분이 작아질 수록 연산량이 많아짐 -> 스택 오버플로우!! 
        // 이를 방지하기 위해 NavMash를 좀 두껍게 처리함... (ㅠㅠ)
        NavMesh.SamplePosition(targetPosition, out hit, 10f, 1);

        // 디버그 찍었을 때 bake 된 영역이 아니면 x,y,z 좌표 전부 Infinity가 뜸!!!
        // Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);

        // bake 된 영역 바깥이면 
        if(hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // 좌표를 새로 갱신 
            UpdatePath();
        }

        targetPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // 회전
        // 회전 업데이트 멈춤
        agent.updateRotation = false;
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;

        // 타겟이 있으면 타겟 바라보기 
        if (targetCharacter!=null)
        {
            transform.LookAt(targetCharacter.transform);
        }

        // agent.updateRotation = true;
    }
    private void TrackingBehaviour()
    {
        transform.LookAt(targetCharacter.transform);

        // 코루틴 진행중이면 강제로 코루틴 멈춤
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }

        // 임시 타겟이 비어있을 때만 코루틴 실행
        if(tempTarget == null)
        {
            if (timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }
        }


        // 애니메이션
        animator.SetFloat("Speed", 0.6f);

        // 다음 목표 좌표를 플레이어로 설정
        targetPosition = targetCharacter.transform.position;

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
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        // 플레이어를 바라보고
        transform.LookAt(targetCharacter.transform);
        agent.velocity = Vector3.zero;

        agent.isStopped = false;

        // 애니메이터
        animator.SetTrigger("Attack");

        // 공격한다
        Debug.Log("AttackBehaviour()");
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


    /// <summary>
    /// 애니메이션 빠져나가기
    /// </summary>
    public void ExitAttack()
    {
        animator.ResetTrigger("Attack");
    }

    public void Cancel()
    {
        ExitAttack();
        // targetCharacter = null;
        agent.isStopped = true; 
    }
}