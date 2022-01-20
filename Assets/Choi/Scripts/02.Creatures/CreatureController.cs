using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;
    /* 상태 */
    public CreatureState state;

    // 컴포넌트
    // private CreaturePatroller patroller;
    // private CreatureTracker tracker;    
    private CreatureMover mover;    
    private CreatureCaster caster;
    private CreatureFighter fighter;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== 체크용 bool 타입 ================ */
    // [SerializeField] bool hasTarget = false; // 타겟유무
    // [SerializeField] bool isCasting = false; // 캐스팅 중인지
    [SerializeField] bool canAttack = false; // 공격할 수 있는지
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    /* ============== 좌표 ================ */
    // 생성 좌표
    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

    /* ============== 시간 ================ */
    // 마지막으로 플레이어를 본 시간
    private float timeSinceLastSawPlayer = 0f;

    /* ============== 코루틴 ================ */
    // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용
    private Coroutine timeLastPatrolCoroutine;



    private NavMeshHit hit;



    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // 패트롤 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, creature.GetPatrolRange());

        // 인식 시야 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, creature.GetTrackingRange());

        // 공격 거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, creature.GetAttackRange());
    }

    #endregion

    #region OnEnable, OnDisable

    private void OnEnable()
    {
        if(!agent.enabled)
        {
            agent.enabled = true;
        }

        // createPosition = CreatuerPool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;

        state = CreatureState.Patrol;
    }
    private void OnDisable()
    {

    }

    #endregion


    private void Awake()
    {
        // patroller = GetComponent<CreaturePatroller>();
        // tracker = GetComponent<CreatureTracker>();
        mover = GetComponent<CreatureMover>();
        caster = GetComponent<CreatureCaster>();
        fighter = GetComponent<CreatureFighter>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //createPosition = CreaturePool.GetInstance().GetCreatePosition();
    }

    private void Update()
    {
        DecideBehaviours();
    }

    /// <summary>
    /// 어떤 행동(Behaviours)을 할지결정
    /// </summary>
    private void DecideBehaviours()
    {
        // 공격 범위에 들어오면
        if (IsInAttackRange())
        {
            if (canAttack)
            {
                AttackBehaviour();
            }
            // 캐스터 컴포넌트가 null이 아니고
            // isCasting이 false 일 때(= 캐스팅 중이 아닐 때)만 실행 
            else if (caster != null && !caster.GetIsCasting())
            {
                CastBehaviour();
            }
        }
        else
        {
            if (!caster.GetIsCasting())
            {
                MoveBehaviour();
            }
        }
    }


    #region CalculateRanges

    /// <summary>
    /// 플레이어와 거리 계산 (공격 범위)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (mover.GetTargetCharacter() == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(mover.GetTargetCharacter().transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 attack 범위보다 적으면 true
        return distanceToPlayer < creature.GetAttackRange();
    }

    #endregion
       

    #region Behaviours()

    private void MoveBehaviour()
    {
        mover.StartPatrolBehaviour();
    }

    private void CastBehaviour()
    {
        mover.Cancel();
        caster.StartSpellCastBehaviour();
    }

    private void AttackBehaviour()
    {
        // caster 컴포넌트가 있으면 caster를 cancel()
        if(caster != null) caster.Cancel();
        // caster 컴포넌트가 없으면 mover를 cancel()
        else mover.Cancel();

        fighter.StartAttackBehaviour();
    }

    #endregion

}
