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
    private CreatureTargetFinder_Test finder;
    private CreatureMover mover;    
    private CreatureCaster caster;
    private CreatureFighter fighter;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== 체크용 bool 타입 ================ */
    // 공격 자체를 할 수 있는지
    [SerializeField] bool canAttack = false;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    // 스킬 공격을 할 수 있는지
    [SerializeField] bool canCasting = false; 
    public bool CanCasting { get { return canCasting; } set { canCasting = value; } }

    /* ============== 좌표 ================ */
    // 생성 좌표
    [SerializeField] Transform createPosition;
    public Transform GetCreatePosition() { return createPosition; }

    /* ============== 시간 ================ */
    // 마지막으로 플레이어를 본 시간
    // private float timeSinceLastSawPlayer = 0f;

    /* ============== 속도 ================ */
    // 애니메이션 속도
    private float animatingSpeed = 1f;
    public float GetAnimatingSpeed() { return animatingSpeed; }




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

        canAttack = false;
        canCasting = false;
    }
    #endregion


    private void Awake()
    {
        finder = GetComponent<CreatureTargetFinder_Test>();
        mover = GetComponent<CreatureMover>();
        caster = GetComponent<CreatureCaster>();
        fighter = GetComponent<CreatureFighter>();

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 공격 범위에 들어오면 = 공격가능
        if (finder.IsInAttackRange()) canAttack = true;
        // 아니라면 = 불가능
        else canAttack = false;

        // 공격 횟수 계산
        CalculateAttackCount();

        // 크리쳐 행동 결정
        DecideBehaviours();

        // 크리쳐 상태에 따른 변화
        ChangeActionFromHpState();
    }

    /// <summary>
    /// 어떤 행동(Behaviours)을 할지결정
    /// </summary>
    private void DecideBehaviours()
    {
        if(canAttack)
        { 
            // 공격도 가능하고 스킬 공격을 할 수 있으면
            if(canCasting)
            {
                // 캐스터 컴포넌트가 null이 아니고
                // isCasting이 false 일 때(= 캐스팅 중이 아닐 때)만 실행 
                if (caster != null && !caster.GetIsCasting())
                {
                    CastBehaviour();
                }
            }            
            else // 공격은 가능하지만 스킬 공격을 할 수 없다면
            {
                if(!fighter.GetIsAttacking())
                {
                    AttackBehaviour();
                }
            }
        }
        else
        {
            if (!caster.GetIsCasting() || !fighter.GetIsAttacking())
            {
                MoveBehaviour();
            }
        }
    }

    /// <summary>
    /// 상태에 따른 행동 변화
    /// </summary>
    private void ChangeActionFromHpState()
    {
        switch(GetComponent<CreatureHp>().GetCurrentCreatureHPState())
        {
            case CreatureHpState.Normal:
                {
                    animatingSpeed = 1f;
                    break;
                }
            case CreatureHpState.Arousal:
                {
                    animatingSpeed = 2f;
                    break;
                }
            case CreatureHpState.Lull:
                {
                    animatingSpeed = 0.6f;
                    break;
                }
            case CreatureHpState.Vaccinable:
                {
                    break;
                }
        }
    }


    #region CalculateRanges, CalculateAttackCounts

    


    /// <summary>
    /// 공격 횟수 계산
    /// </summary>
    private void CalculateAttackCount()
    {
        Debug.Log("CalculateAttackCount");
        if (fighter.AttackCount > 2)
        {
            // 캐스팅 가능 상태
            GetComponent<CreatureController>().CanCasting = true;
            // 공격횟수 0으로 초기화
            fighter.AttackCount = 0;
        }
    }

    #endregion


    #region Behaviours()

    private void MoveBehaviour()
    {
        mover.StartPatrolBehaviour();
    }

    private void CastBehaviour()
    {
        caster.StartSpellCastBehaviour();
    }

    private void AttackBehaviour()
    {
        fighter.StartAttackBehaviour();
    }

    #endregion

}
