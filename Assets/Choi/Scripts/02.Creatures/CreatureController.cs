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
    // private CreaturePatroller patroller;
    // private CreatureTracker tracker;    
    private CreatureMover mover;    
    private CreatureCaster caster;
    private CreatureFighter fighter;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== üũ�� bool Ÿ�� ================ */
    // [SerializeField] bool hasTarget = false; // Ÿ������
    // [SerializeField] bool isCasting = false; // ĳ���� ������
    [SerializeField] bool canAttack = false; // ������ �� �ִ���
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    /* ============== ��ǥ ================ */
    // ���� ��ǥ
    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

    /* ============== �ð� ================ */
    // ���������� �÷��̾ �� �ð�
    private float timeSinceLastSawPlayer = 0f;

    /* ============== �ڷ�ƾ ================ */
    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;



    private NavMeshHit hit;



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
    /// � �ൿ(Behaviours)�� ��������
    /// </summary>
    private void DecideBehaviours()
    {
        // ���� ������ ������
        if (IsInAttackRange())
        {
            if (canAttack)
            {
                AttackBehaviour();
            }
            // ĳ���� ������Ʈ�� null�� �ƴϰ�
            // isCasting�� false �� ��(= ĳ���� ���� �ƴ� ��)�� ���� 
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
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (mover.GetTargetCharacter() == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(mover.GetTargetCharacter().transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� attack �������� ������ true
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
        // caster ������Ʈ�� ������ caster�� cancel()
        if(caster != null) caster.Cancel();
        // caster ������Ʈ�� ������ mover�� cancel()
        else mover.Cancel();

        fighter.StartAttackBehaviour();
    }

    #endregion

}
