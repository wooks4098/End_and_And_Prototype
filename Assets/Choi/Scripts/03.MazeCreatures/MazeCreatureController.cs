using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureController : MonoBehaviour
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ������Ʈ
    private MazeCreatureMover mover;
    private MazeCreatureFighter fighter;

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
    public Transform GetCreatePosition() { return createPosition; }

    /* ============== �ð� ================ */
    // ���������� �÷��̾ �� �ð�
    private float timeSinceLastSawPlayer = 0f;


    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // ��Ʈ�� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, 3f);

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
            AttackBehaviour();
        }
        else
        {
            MoveBehaviour();
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
