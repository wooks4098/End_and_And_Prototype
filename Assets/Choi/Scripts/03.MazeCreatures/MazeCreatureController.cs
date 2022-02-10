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
    private CreatureTargetFinder finder;

    private Animator animator;
    private NavMeshAgent agent;

    /* ============== üũ�� bool Ÿ�� ================ */
    [SerializeField] bool canAttack = false; // ������ �� �ִ���
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    /* ============== ��ǥ ================ */
    // ���� ��ǥ
    [SerializeField] Transform createPosition;
    public Transform GetCreatePosition() { return createPosition; }


    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
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
        // ���� ������ ������ = ���ݰ���
        if (finder.IsInAttackRange()) canAttack = true;
        // �ƴ϶�� = �Ұ���
        else canAttack = false;

        // ũ���� �ൿ ����
        DecideBehaviours();
    }

    /// <summary>
    /// � �ൿ(Behaviours)�� ��������
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
