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

    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 5f;

    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    // ���������� �÷��̾ �� �ð�
    private float timeSinceLastSawPlayer = 0f;

    private Coroutine waitNextPatrolCoroutine;
    private Coroutine timeLastPatrolCoroutine;



    #region OnDrawGizmos

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
        Gizmos.DrawWireSphere(transform.position, creature.attackRange);

        // ���� (��ǥ) ��ġ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(nextPosition, 3);
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

        timeForWaitingPatrol = 5f;
    }

    private void Update()
    {
        if (!isActive) return;

        // ���� ������ ������
        if(IsInAttackRange())
        {
            AttackBehaviour();
        }
        // Ʈ��ŷ ������ ������
        else if (IsInTrackingRange())
        {
            TrackingBehaviour();
        }
        // ���� �ƴϸ� ��Ʈ��
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
            // ������ ��Ʈ�� �ð� 0���� �ʱ�ȭ
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol());
            }            
        }
        else
        {
            if(timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }            
        }
    }

    private IEnumerator TimeLastPatrol()
    {
        while(true)
        {
            // ���������� ��Ʈ������
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 3�ʸ� �ʰ��ϸ�
            if (timeSinceLastPatrol > 10f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                UpdatePath();
                // ����������
                break;
            }
        }

        // �ٽ� �ð��� 0����
        timeSinceLastPatrol = 0f;
        // �ڷ�ƾ ����
        timeLastPatrolCoroutine = null;
    }

    private IEnumerator WaitNextPatrol()
    {
        while (true)
        {
            timeForWaitingPatrol -= Time.deltaTime;

            yield return new WaitForFixedUpdate();

            if(timeForWaitingPatrol < 0f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                UpdatePath();
                // ����������
                break;
            }
        }
        // �ð��� 5��
        timeForWaitingPatrol = 5f;
        // �ڷ�ƾ ����
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼���
    /// </summary>
    private void UpdatePath()
    {
        // ���� ��ǥ�� ��ǥ ��ǥ�� ����
        currentPosition = nextPosition;

        // ���� X, Z ��ǥ ���� - CreatePosition�� �߽�����
        // createPosition - creature.patrolRange => (���� ������ - ũ���� ��Ʈ�� ����)
        // 
        float randomX = UnityEngine.Random.Range(createPosition.x - creature.patrolRange, createPosition.x + creature.patrolRange);
        float randomZ = UnityEngine.Random.Range(createPosition.z - creature.patrolRange, createPosition.z + creature.patrolRange);
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
    private void TrackingBehaviour()
    {
        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        nextPosition = player.transform.position;

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
        agent.velocity = Vector3.zero;

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
        // Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.trackingRange;
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
}