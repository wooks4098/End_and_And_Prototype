using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ũ������ �������� �����ϴ� Ŭ����
/// </summary>
public class CreatureMovement : MonoBehaviour, ICreatureAction
{
    [SerializeField] Animator animator;

    [SerializeField] bool isActive;
    [SerializeField] bool hasTarget;
    // [SerializeField] bool isAttacking;

    [SerializeField] Creature creature;
    private NavMeshAgent agent;

    [SerializeField] CreaturePlayer targetCharacter; // ���� Ÿ��
    [SerializeField] CreaturePlayer tempTarget; // �ӽ÷� ������ Ÿ��

    [SerializeField] Transform createPosition;
    private Vector3 targetPosition;

    // ���� ������ �þ��� ������ (���� ������ ����)
    // public Transform eyeTransform;

    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 5f;

    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    // ���������� �÷��̾ �� �ð�
    private float timeSinceLastSawPlayer = 0f;

    private Coroutine waitNextPatrolCoroutine; // ���� ��Ʈ���� ��ٸ��� �ڷ�ƾ
    private Coroutine timeLastPatrolCoroutine; // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���

    // path
    // NavMeshPath path;
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

    #region OnEnable, OnDisable

    private void OnEnable()
    {
        if(!agent.enabled)
        {
            agent.enabled = true;
        }

        //createPosition = CreaturePool.GetInstance().GetCreatePosition();
        transform.position = createPosition.position;

        creature.state = CreatureState.Patrol;
        timeForWaitingPatrol = 5f;

        //currentPosition = 

        targetPosition = createPosition.position;

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;
    }

    private void OnDisable()
    {
        
    }

    #endregion


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // path = new NavMeshPath();
    }
    private void Start()
    {
        // agent.stoppingDistance�� �̿��ϸ� ��� ���� �Ÿ����� ������ ������ �� �ִ�.
        // agent.stoppingDistance = creature.attackDistance;

        timeForWaitingPatrol = 5f;
    }

    private void Update()
    {
        FindTargetCharacter();

        if (hasTarget)
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
        }
        else
        {
            creature.state = CreatureState.Patrol;
        }

        DecisionBehaviour(creature.state);
    }

    /// <summary>
    ///  Ÿ�� ĳ���� ����
    /// </summary>
    private void FindTargetCharacter()
    {
        // Cancel();

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.trackingRange);

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("���� ã�ҽ��ϴ�!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
            if(activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {

                // ���� �ӽ� Ÿ���� ������� �ʰ� (-> �ӽ� Ÿ���� ������� ���� ���� ��)
                // �ӽ� Ÿ���� ������ ���Ӱ� ���ϴ� Ÿ���� ������ ��
                if (tempTarget != null
                    && tempTarget.score > activeCollider.gameObject.GetComponent<CreaturePlayer>().score)
                {
                    // �ӽ� Ÿ�� ���� ������ ũ�� ��� (continue)
                    continue;
                }
                else
                {
                    // �ӽ�Ÿ�� ����
                    tempTarget = activeCollider.gameObject.GetComponent<CreaturePlayer>();

                    // �ִϸ��̼� ����
                    animator.SetBool("Attack", false);                    
                }

                // Ÿ�� ����
                targetCharacter = tempTarget;
                agent.isStopped = false;

                hasTarget = true;                
            }

            else
            {
                // �ӽ�Ÿ���� ����
                tempTarget = null;
            }
        }        
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
        agent.updateRotation = true;

        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.1f);

        // ������ �����ߴ���
        if (IsArrive())
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
    /// ��Ʈ�� �ð��� �󸶳� ���������� ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeLastPatrol()
    {
        while (true)
        {
            // �ӽ� Ÿ���� ����� ���� ������ ��������
            if (tempTarget != null) break;

            // ���������� ��Ʈ������
            timeSinceLastPatrol += Time.deltaTime;

            yield return new WaitForFixedUpdate();

            // 10�ʸ� �ʰ��ϸ�
            if (timeSinceLastPatrol > 10f)
            {
                // Ÿ���� ã�ƺ���
                // FindTargetCharacter();

                // �ӽ� Ÿ���� ������
                if(tempTarget == null)
                {
                    // 1. Ÿ���� �������� ǥ��
                    hasTarget = false;

                    // 2. ���� Ÿ���� ����
                    targetCharacter = null;

                    // 3. ���ο� ��ǥ�� ����
                    UpdatePath();

                    // ����������
                    break;
                }
            }
        }

        // �ٽ� �ð��� 0����
        timeSinceLastPatrol = 0f;
        // �ڷ�ƾ ����
        timeLastPatrolCoroutine = null;
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
                UpdatePath();
                // ����������
                break;
            }
        }
        // �ð��� 5��
        timeForWaitingPatrol = 2f;
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
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        // ��ǥ�� �̸� ����غ��� hit�� ��ȯ
        // ((����)) �Ű����� maxDistance �κ��� �۾��� ���� ���귮�� ������ -> ���� �����÷ο�!! 
        // �̸� �����ϱ� ���� NavMash�� �� �β��� ó����... (�Ф�)
        NavMesh.SamplePosition(targetPosition, out hit, 10f, 1);

        // ����� ����� �� bake �� ������ �ƴϸ� x,y,z ��ǥ ���� Infinity�� ��!!!
        // Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);

        // bake �� ���� �ٱ��̸� 
        if(hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // ��ǥ�� ���� ���� 
            UpdatePath();
        }

        targetPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // ȸ��
        // ȸ�� ������Ʈ ����
        agent.updateRotation = false;
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        agent.destination = targetPosition;
        agent.speed = creature.patrolSpeed;

        // Ÿ���� ������ Ÿ�� �ٶ󺸱� 
        if (targetCharacter!=null)
        {
            transform.LookAt(targetCharacter.transform);
        }

        // agent.updateRotation = true;
    }
    private void TrackingBehaviour()
    {
        transform.LookAt(targetCharacter.transform);

        // �ڷ�ƾ �������̸� ������ �ڷ�ƾ ����
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }

        // �ӽ� Ÿ���� ������� ���� �ڷ�ƾ ����
        if(tempTarget == null)
        {
            if (timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }
        }


        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.6f);

        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        targetPosition = targetCharacter.transform.position;

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
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        // �÷��̾ �ٶ󺸰�
        transform.LookAt(targetCharacter.transform);
        agent.velocity = Vector3.zero;

        agent.isStopped = false;

        // �ִϸ�����
        animator.SetTrigger("Attack");

        // �����Ѵ�
        Debug.Log("AttackBehaviour()");
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


    /// <summary>
    /// �ִϸ��̼� ����������
    /// </summary>
    public void ExitAttack()
    {
        animator.ResetTrigger("Attack");
    }

    public void Cancel()
    {
        ExitAttack();
        // targetCharacter = null;
        agent.isStopped = true; 
    }
}