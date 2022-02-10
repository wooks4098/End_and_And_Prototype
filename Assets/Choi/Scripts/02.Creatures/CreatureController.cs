using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;
    /* ���� */
    public CreatureState state;

    // ������Ʈ   
    private CreatureTargetFinder_Test finder;
    private CreatureMover mover;    
    private CreatureCaster caster;
    private CreatureFighter fighter;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== üũ�� bool Ÿ�� ================ */
    // ���� ��ü�� �� �� �ִ���
    [SerializeField] bool canAttack = false;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    // ��ų ������ �� �� �ִ���
    [SerializeField] bool canCasting = false; 
    public bool CanCasting { get { return canCasting; } set { canCasting = value; } }

    /* ============== ��ǥ ================ */
    // ���� ��ǥ
    [SerializeField] Transform createPosition;
    public Transform GetCreatePosition() { return createPosition; }

    /* ============== �ð� ================ */
    // ���������� �÷��̾ �� �ð�
    // private float timeSinceLastSawPlayer = 0f;

    /* ============== �ӵ� ================ */
    // �ִϸ��̼� �ӵ�
    private float animatingSpeed = 1f;
    public float GetAnimatingSpeed() { return animatingSpeed; }




    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // ��Ʈ�� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, creature.GetPatrolRange());

        // �ν� �þ� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, creature.GetTrackingRange());

        // ���� �Ÿ�
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
        // ���� ������ ������ = ���ݰ���
        if (finder.IsInAttackRange()) canAttack = true;
        // �ƴ϶�� = �Ұ���
        else canAttack = false;

        // ���� Ƚ�� ���
        CalculateAttackCount();

        // ũ���� �ൿ ����
        DecideBehaviours();

        // ũ���� ���¿� ���� ��ȭ
        ChangeActionFromHpState();
    }

    /// <summary>
    /// � �ൿ(Behaviours)�� ��������
    /// </summary>
    private void DecideBehaviours()
    {
        if(canAttack)
        { 
            // ���ݵ� �����ϰ� ��ų ������ �� �� ������
            if(canCasting)
            {
                // ĳ���� ������Ʈ�� null�� �ƴϰ�
                // isCasting�� false �� ��(= ĳ���� ���� �ƴ� ��)�� ���� 
                if (caster != null && !caster.GetIsCasting())
                {
                    CastBehaviour();
                }
            }            
            else // ������ ���������� ��ų ������ �� �� ���ٸ�
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
    /// ���¿� ���� �ൿ ��ȭ
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
    /// ���� Ƚ�� ���
    /// </summary>
    private void CalculateAttackCount()
    {
        Debug.Log("CalculateAttackCount");
        if (fighter.AttackCount > 2)
        {
            // ĳ���� ���� ����
            GetComponent<CreatureController>().CanCasting = true;
            // ����Ƚ�� 0���� �ʱ�ȭ
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
