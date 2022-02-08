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
    // 공격 자체를 할 수 있는지
    [SerializeField] bool canAttack = false;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    // 스킬 공격을 할 수 있는지
    [SerializeField] bool canUseSkill = false; 
    public bool CanUseSkill { get { return canUseSkill; } set { canUseSkill = value; } }

    /* ============== 좌표 ================ */
    // 생성 좌표
    [SerializeField] Transform createPosition;
    public Transform GetCreatePosition() { return createPosition; }

    /* ============== 시간 ================ */
    // 마지막으로 플레이어를 본 시간
    private float timeSinceLastSawPlayer = 0f;

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

        canUseSkill = false;
        canUseSkill = false;
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

    private void Update()
    {
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
        // 공격 범위에 들어오면
        if (IsInAttackRange())
        {
            // 캐스터 컴포넌트가 null이 아니고
            // isCasting이 false 일 때(= 캐스팅 중이 아닐 때)만 실행 
            if (caster != null && !caster.GetIsCasting())
            {
                CastBehaviour();
            }
            else if (canAttack)
            {
                AttackBehaviour();
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
