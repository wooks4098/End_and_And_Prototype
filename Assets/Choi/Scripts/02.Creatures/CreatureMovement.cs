using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ũ������ �������� �����ϴ� Ŭ����
/// </summary>
public class CreatureMovement : MonoBehaviour
{
    [SerializeField] bool isActive;
    [SerializeField] bool hasTarget;

    [SerializeField] Creature creature;
    private NavMeshAgent agent;

    [SerializeField] CreaturePlayer player;

    [SerializeField] Vector3 createPosition;
    private Vector3 currentPosition;
    private Vector3 nextPosition;

    // ���� ������ �þ��� ������ (���� ������ ����)
    public Transform eyeTransform;

    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    // ���������� �÷��̾ �� �ð�
    [SerializeField] float timeSinceLastSawPlayer = 0f;


    #region Gizmos

    private void OnDrawGizmos()
    {
        // ��Ʈ�� ����
        Gizmos.color = Color.blue;        
        Gizmos.DrawWireSphere(createPosition, creature.patrolRange);

        // �ν� �þ� ����
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, creature.trackingRange);

        // ���� �Ÿ�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, creature.attackDistance);

        // ���� �Ÿ�
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(nextPosition, 5);
    }

    #endregion

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        currentPosition = createPosition;
        nextPosition = currentPosition;

        // agent.stoppingDistance�� �̿��ϸ� ��� ���� �Ÿ����� ������ ������ �� �ִ�.
        // agent.stoppingDistance = creature.attackDistance;

        //PatrolBehaviour();
    }

    private void Update()
    {
        if (!isActive) return;

        if(IsInAttackRange())
        {
            AttackBehaviour();
        }
        else if (IsInTrackingRange())
        {
            TrackingBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        // UpdateTimers();
    }

    /// <summary>
    /// ��Ʈ�� �ൿ
    /// </summary>
    private void PatrolBehaviour()
    {
        // ������ �����ߴ���
        if(IsArrive())
        {
            UpdatePath();
        }
        else
        {
            // ���������� ��Ʈ������
            timeSinceLastPatrol += Time.deltaTime;
            // 3�ʸ� �ʰ��ϸ�
            if(timeSinceLastPatrol > 7f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                UpdatePath();

                // �ٽ� �ð��� 0����
                timeSinceLastPatrol = 0f;
            }
        }
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼���
    /// </summary>
    private void UpdatePath()
    {
        // ���� X, Z ��ǥ ���� - CreatePosition�� �߽�����
        float randomX = UnityEngine.Random.Range(0, createPosition.x);
        float randomZ = UnityEngine.Random.Range(0, createPosition.z);
        // ������ ������ ����
        Vector3 movePosition = new Vector3(randomX, createPosition.y, randomZ);

        // ���� ��ǥ ��ǥ�� movePosition���� �Ҵ�
        nextPosition = movePosition;
        // �ڵ� ���� �Ÿ��� 0���� �����Ǿ����� ���� ���
        if(agent.stoppingDistance != 0f)
        {
            // 0���� �ʱ�ȭ ���ش�.
            agent.stoppingDistance = 0f;
        }
        // ������ ��ǥ�� nextPosition���� �Ҵ�
        agent.destination = nextPosition;
        agent.speed = creature.patrolSpeed;
    }

    /// <summary>
    /// �����ߴ��� �Ǻ��ϴ� �޼���
    /// </summary>
    private bool IsArrive()
    {
        // ���� �����ǰ� ũ������ �Ÿ� ���
        float distanceToWaypoint = Vector3.Distance(transform.position, nextPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint < 1f;
    }

    private void TrackingBehaviour()
    {
        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        nextPosition = player.transform.position;

        // agent.stoppingDistance�� �̿��ϸ� ��� ���� �Ÿ����� ������ ������ �� �ִ�.
        agent.stoppingDistance = creature.attackDistance;
        // ���� ��ǥ�� �̵�
        agent.destination = nextPosition;
        // Ʈ��ŷ �ӵ��� ��ȯ
        agent.speed = creature.trackingSpeed;
    }

    /// <summary>
    /// ���� �ൿ
    /// </summary>
    private void AttackBehaviour()
    {
        // �÷��̾ �ٶ󺸰�
        transform.LookAt(player.transform);
        // �����Ѵ�
        Debug.Log("AttackBehaviour()");
    }

    /// <summary>
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    private bool IsInAttackRange()
    {
        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� attack �������� ������ true
        return distanceToPlayer < creature.attackRange;
    }

    /// <summary>
    /// �÷��̾���� �Ÿ� ��� (Ʈ��ŷ ����)
    /// </summary>
    private bool IsInTrackingRange()
    {
        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.trackingRange;
    }
}
