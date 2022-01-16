using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    //CreatureMovement patroller;
    CreaturePatroller patroller;
    CreatureTracker tracker;    
    CreatureCaster caster;
    CreatureFighter fighter;

    [SerializeField] bool hasTarget;
    [SerializeField] bool isCasting = false;
    [SerializeField] bool canAttack = false;


    [SerializeField] Creature creature;

    [SerializeField] CreaturePlayer tempTarget;
    [SerializeField] CreaturePlayer targetCharacter;

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;
    
    #region OnDrawGizmos

    private void OnDrawGizmos()
    {
        // ��Ʈ�� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(createPosition.position, creature.patrolRange);

        // �ν� �þ� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, creature.trackingRange);

        // ���� �Ÿ�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, creature.attackRange);

        // ���� (��ǥ) ��ġ
        // Gizmos.color = Color.cyan;
        // Gizmos.DrawWireSphere(targetPosition.position, 3);
    }

    #endregion

    private void Awake()
    {
        patroller = GetComponent<CreaturePatroller>();
        tracker = GetComponent<CreatureTracker>();
        caster = GetComponent<CreatureCaster>();
        fighter = GetComponent<CreatureFighter>();
    }

    private void Start()
    {
        //createPosition = CreaturePool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;
    }

    private void Update()
    {
        if (hasTarget)
        {
            // ���� ������ ������
            if (IsInAttackRange())
            {
                if (!canAttack)
                {
                    CastBehaviour();
                }
                else
                {
                    AttackBehaviour();
                }
            }
            // Ʈ��ŷ ������ ������
            else if (IsInTrackingRange())
            {
                if (!isCasting)
                {
                    TrackBehaviour();
                }
                else if (canAttack)
                {
                    AttackBehaviour();
                }
            }
        }
        else
        {
            PatrolBehaviour();
        }
    }


    /// <summary>
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    private bool IsInAttackRange()
    {
        if (targetCharacter == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(targetCharacter.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� attack �������� ������ true
        return distanceToPlayer < creature.attackRange;
    }

    /// <summary>
    /// �÷��̾���� �Ÿ� ��� (Ʈ��ŷ ����)
    /// </summary>
    private bool IsInTrackingRange()
    {
        if (targetCharacter == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(targetCharacter.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.trackingRange;
    }


    private void PatrolBehaviour()
    {
        patroller.StartPatrolBehaviour(createPosition);
    }

    private void TrackBehaviour()
    {
        tracker.StartTrackingBehaviour(tempTarget, targetCharacter);
    }

    private void CastBehaviour()
    {
        caster.StartSpellCastBehaviour();
    }

    private void AttackBehaviour()
    {
        fighter.StartAttackBehaviour();
    }
}
