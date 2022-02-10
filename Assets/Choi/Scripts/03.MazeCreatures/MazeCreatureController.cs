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
    private CreatureTargetFinder finder;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== 체크용 bool 타입 ================ */
    [SerializeField] bool canAttack = false; // 공격할 수 있는지
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    /* ============== 좌표 ================ */
    // 생성 좌표
    [SerializeField] Transform createPosition;
    public Transform GetCreatePosition() { return createPosition; }


    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
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

        canAttack = false;
    }

    #endregion

    private void Awake()
    {
        finder = GetComponent<CreatureTargetFinder>();
        mover = GetComponent<MazeCreatureMover>();
        fighter = GetComponent<MazeCreatureFighter>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 공격 범위에 들어오면 = 공격가능
        if (finder.IsInAttackRange()) canAttack = true;
        // 아니라면 = 불가능
        else canAttack = false;

        // 크리쳐 행동 결정
        DecideBehaviours();
    }

    /// <summary>
    /// 어떤 행동(Behaviours)을 할지결정
    /// </summary>
    private void DecideBehaviours()
    {
        if(canAttack)
        {
            if(!fighter.GetIsAttacking())
            {
                AttackBehaviour();
            }
        }
        else
        {
            if (!fighter.GetIsAttacking())
            {
                MoveBehaviour();
            }                
        }
    }

    #region Behaviours()

    private void MoveBehaviour()
    {
        // fighter.Cancel();

        mover.StartPatrolBehaviour();
    }

    private void AttackBehaviour()
    {
        // mover.Cancel();

        fighter.StartAttackBehaviour();
    }

    #endregion
}
