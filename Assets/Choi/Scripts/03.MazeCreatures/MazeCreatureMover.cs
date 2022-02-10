using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureMover : MonoBehaviour, ICreatureAction
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ������Ʈ
    private Animator animator;
    private NavMeshAgent agent;
    private CreatureTargetFinder finder;

    /* ============== �ð� ================ */
    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 2f;
    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== �ڷ�ƾ ================ */
    // ���� ��Ʈ���� ��ٸ��� �ڷ�ƾ
    private Coroutine waitNextPatrolCoroutine;
    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;

    // Ÿ�� ������
    Transform targetPosition1;
    Transform targetPosition2;

    /* ============== ��ǥ/������ ================ */
    // ��Ʈ�� �� �ڸ�
    [SerializeField] MazePatrolPath patrolPath;
    // ���� index
    private int currentWaypointIndex;
    // ���� ������
    private Vector3 v3nextPosition;    



    #region DrawGizmos
    private void OnDrawGizmos()
    {
        // ���� (��ǥ) ��ġ
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(v3nextPosition, 0.2f);
    }

    #endregion
    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 3f;

        v3nextPosition = GetCurrentWaypoint();
    }
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        finder = GetComponent<CreatureTargetFinder>();
    }

    private void Start()
    {
        targetPosition1 = GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer);
        targetPosition2 = GameManager.Instance.GetPlayerTrans(PlayerType.SecondPlayer);        
    }

    private void Update()
    {
        finder.FindTarget();

        if (finder.IsInTrackingRange() && !finder.IsInAttackRange())
        {
            StartTrackingBehaviour();
        }
    }

    /// <summary>
    /// StartMoveBehavour?
    /// </summary>
    public void StartPatrolBehaviour()
    {
        Debug.Log("Mover.StartPatrolBehaviour()");

        GetComponent<CreatureActionScheduler>().StartAction(this);

        // ��Ʈ��
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPath == null) return;

        // ������Ʈ ����
        agent.updateRotation = true;
        agent.isStopped = false;
        // ������Ʈ ��ǥ ����        
        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.1f);

        // ������ ��ġ�� �����ߴ���
        if (AtWaypoint())
        {
            // ���������� 1. �ִϸ��̼� Idle��
            animator.SetFloat("Speed", 0.0f);
            // 2. ����
            agent.velocity = Vector3.zero;

            // ������ ��Ʈ�� �ð� 0���� �ʱ�ȭ
            timeSinceLastPatrol = 0f;

            // �ڷ�ƾ ����
            if (waitNextPatrolCoroutine == null)
            {
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol());
            }     
        }
    }

    /// <summary>
    /// �����ߴ��� �Ǻ��ϴ� �޼���
    /// </summary>
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, v3nextPosition);
        return distanceToWaypoint <= 0.1f;
    }
    /// <summary>
    /// �е�� �� �ε��� ��ȯ (-> ���ο� �ε����� ����)
    /// </summary>
    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }
    /// <summary>
    /// ���ο� �ε����� ���� ���� ��ǥ�� ��´�
    /// nextPosition = GetCurrentWayPoint();
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    /// <summary>
    /// ���� ��Ʈ�� ��ǥ�� ã����� �ð��� ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitNextPatrol()
    {
        while (true)
        {
            timeForWaitingPatrol -= Time.deltaTime;

            yield return new WaitForFixedUpdate();

            if (timeForWaitingPatrol < 0f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                CycleWaypoint(); // �ε��� ����
                v3nextPosition = GetCurrentWaypoint(); // ���ο� �ε����� ���� ������ǥ ����
               
                break; // ����������
            }
        }
        // �ð��� 2��
        timeForWaitingPatrol = 2f;
        // �ڷ�ƾ ����
        waitNextPatrolCoroutine = null;
    }

    public void StartTrackingBehaviour()
    {
        Debug.Log("Mover.StartTrackingBehaviour()");
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        Tracking();
    }
    private void Tracking()
    {
        transform.LookAt(finder.GetTarget().transform);

        // Ÿ���� ������� ���� �ڷ�ƾ ����
        if (finder.GetTarget() == null)
        {
            if (timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }
        }

        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.6f);

        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        // v3nextPosition = finder.GetTarget().transform.position;
        if(finder.GetTarget().CompareTag("Player1"))
        {
            // ��ǥ
            v3nextPosition = GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer).position;
        }
        else if (finder.GetTarget().CompareTag("Player2"))
        {
            // ��ǥ
            v3nextPosition = GameManager.Instance.GetPlayerTrans(PlayerType.SecondPlayer).position;
        }

        // ���� ��ǥ�� �̵�
        agent.destination = v3nextPosition;

        // Ʈ��ŷ �ӵ��� ��ȯ
        agent.speed = creature.GetTrackingSpeed();
    }

    private IEnumerator TimeLastPatrol()
    {
        while (true)
        {
            // �ӽ� Ÿ���� ����� ���� ������ ��������
            if (finder.GetTarget() != null) break;

            // ���������� ��Ʈ������
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 10�ʸ� �ʰ��ϸ�
            if (timeSinceLastPatrol > 10f)
            {
                // Ÿ���� ã�ƺ���
                finder.FindTarget();

                // �ӽ� Ÿ���� ������
                if (finder.GetTarget() == null)
                {
                    // ����������
                    break;
                }
            }
        }
    }

    public void Cancel()
    {
        Debug.Log("Mover.Cancel()");

        // �ڷ�ƾ �������̸� ������ �ڷ�ƾ ����
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }

        // �ð� �ʱ�ȭ
        timeForWaitingPatrol = 3f;

        // target�� ����
        // trackingTargetCharacter = null;

        // agent �ʱ�ȭ
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}
