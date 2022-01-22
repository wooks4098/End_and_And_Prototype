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

    /* ============== üũ�� bool Ÿ�� ================ */
    [SerializeField] bool hasTarget = false; // Ÿ������

    /* ============== Ÿ�� ================ */
    // ���� Ÿ��
    [SerializeField] CreaturePlayer trackingTargetCharacter;
    public CreaturePlayer GetTargetCharacter() { return trackingTargetCharacter; }

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
        Gizmos.DrawWireSphere(v3nextPosition, 3f);
    }

    #endregion
    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 5f;

        v3nextPosition = GetCurrentWaypoint();
    }
    private void OnDisable()
    {

    }

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        FindTrackingTargetCharacter();

        if (IsInTrackingRange())
        {
            StartTrackingBehaviour();
        }
    }
    
    /// <summary>
    /// �÷��̾���� �Ÿ� ��� (Ʈ��ŷ ����)
    /// </summary>
    private bool IsInTrackingRange()
    {
        if (trackingTargetCharacter == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(trackingTargetCharacter.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.GetTrackingRange();
    }

    /// <summary>
    /// StartMoveBehavour?
    /// </summary>
    public void StartPatrolBehaviour()
    {
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
        return distanceToWaypoint <= 2.6f;
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
        transform.LookAt(trackingTargetCharacter.transform);

        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.6f);

        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        v3nextPosition = trackingTargetCharacter.transform.position;

        // ���� ��ǥ�� �̵�
        agent.destination = v3nextPosition;
        // Ʈ��ŷ �ӵ��� ��ȯ
        agent.speed = creature.GetTrackingSpeed();
    }

    /// <summary>
    ///  �Ѿư� Ÿ�� ĳ���� ã��
    /// </summary>
    private void FindTrackingTargetCharacter()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("���� ã�ҽ��ϴ�!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
            if (activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {                                
                agent.isStopped = false;

                trackingTargetCharacter = activeCollider.GetComponent<CreaturePlayer>();

                hasTarget = true;
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
        timeForWaitingPatrol = 2f;

        // target�� ����
        trackingTargetCharacter = null;

        // agent �ʱ�ȭ
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}
