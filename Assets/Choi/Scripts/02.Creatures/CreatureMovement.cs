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

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

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

    // path
    NavMeshPath path;
    private NavMeshHit hit;

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
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }
    private void Start()
    {
        targetPosition = createPosition.position;

        // agent.stoppingDistance�� �̿��ϸ� ��� ���� �Ÿ����� ������ ������ �� �ִ�.
        // agent.stoppingDistance = creature.attackDistance;

        timeForWaitingPatrol = 5f;
    }

    private void Update()
    {
        // ���� ������ ������
        if (IsInAttackRange())
        {
            creature.state = CreatureState.Attack;
        }
        // Ʈ��ŷ ������ ������
        else if (IsInTrackingRange())
        {
            creature.state = CreatureState.Tracking;
        }
        else
        {
            creature.state = CreatureState.Patrol;
        }

        DecisionBehaviour(creature.state);
    }

    private void DecisionBehaviour(CreatureState _state)
    {
        switch (_state)
        {
            case CreatureState.Patrol:
                PatrolBehaviour();
                break;
            case CreatureState.Tracking:
                TrackingBehaviour();
                break;
            case CreatureState.Attack:
                AttackBehaviour();
                break;
            default:
                PatrolBehaviour();
                break;
        }
    }

    /// <summary>
    /// ��Ʈ�� �ൿ
    /// </summary>
    private void PatrolBehaviour()
    {
        // ������ �����ߴ���
        if (IsArrive())
        {
            // ������ ��Ʈ�� �ð� 0���� �ʱ�ȭ
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol());
            }
        }
    }

    private IEnumerator TimeLastPatrol()
    {
        while (true)
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

            if (timeForWaitingPatrol < 0f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                UpdatePath();
                // ����������
                break;
            }
        }
        // �ð��� 5��
        timeForWaitingPatrol = 1f;
        // �ڷ�ƾ ����
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼���
    /// </summary>
    private void UpdatePath()
    {

        // ���� X, Z ��ǥ ���� - CreatePosition�� �߽�����
        // createPosition - creature.patrolRange => (���� ������ - ũ���� ��Ʈ�� ����)        
        float randomX = UnityEngine.Random.Range(createPosition.position.x - creature.patrolRange, createPosition.position.x + creature.patrolRange);
        float randomZ = UnityEngine.Random.Range(createPosition.position.z - creature.patrolRange, createPosition.position.z + creature.patrolRange);

        // �������� ������ ������ ����
        targetPosition = new Vector3(randomX, 2.5f, randomZ);

        // ��ǥ�� �̸� ����غ��� hit�� ��ȯ
        // ((����)) �Ű����� maxDistance �κ��� �۾��� ���� ���귮�� ������ -> ���� �����÷ο�!! 
        // �̸� �����ϱ� ���� NavMash�� �� �β��� ó����... (�Ф�)
        NavMesh.SamplePosition(targetPosition, out hit, 5f, 1);

        // ����� ����� �� bake �� ������ �ƴϸ� x,y,z ��ǥ ���� Infinity�� ��!!!
        Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);
        // bake �� ���� �ٱ��̸� 
        if(hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // ��ǥ�� ���� ���� 
            UpdatePath();
        }

        targetPosition = hit.position;
        Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;
    }
    private void TrackingBehaviour()
    {
        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        targetPosition = player.transform.position;

        // ���� ��ǥ�� �̵�
        agent.destination = targetPosition;
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
        float distanceToWaypoint = Vector3.Distance(transform.position, targetPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }


    public bool IsAgentOnNavMesh(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);

        if (!agent.pathPending)
        {
            return false;
        }

        return true;
    }
}