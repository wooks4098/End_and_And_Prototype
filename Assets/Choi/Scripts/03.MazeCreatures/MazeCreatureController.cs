using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureController : MonoBehaviour
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    // 컴포넌트
    private MazeCreatureMover mover;
    private MazeCreatureFighter fighter;

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
    public Transform GetCreatePosition() { return createPosition; }

    /* ============== 시간 ================ */
    // 마지막으로 플레이어를 본 시간
    // private float timeSinceLastSawPlayer = 0f;


    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // 패트롤 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, 3f);

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
        if (!agent.enabled)
        {
            agent.enabled = true;
        }

        // createPosition = CreatuerPool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;
    }

    #endregion

    private void Awake()
    {
        // patroller = GetComponent<CreaturePatroller>();
        // tracker = GetComponent<CreatureTracker>();
        mover = GetComponent<MazeCreatureMover>();
        fighter = GetComponent<MazeCreatureFighter>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GetComponent<CreatureHp>().GetIsDead()) return;

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
            AttackBehaviour();
        }
        else
        {
            MoveBehaviour();
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
        fighter.Cancel();

        mover.StartPatrolBehaviour();
    }

    private void AttackBehaviour()
    {
        // mover.Cancel();

        fighter.StartAttackBehaviour();
    }

    #endregion
}
