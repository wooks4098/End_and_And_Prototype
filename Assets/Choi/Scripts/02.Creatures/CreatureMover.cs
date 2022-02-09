using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureMover : MonoBehaviour, ICreatureAction
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ������Ʈ
    private Animator animator;
    private NavMeshAgent agent;

    private CreatureTargetFinder finder;

    /* ============== �ð� ================ */
    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 5f;
    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== �ڷ�ƾ ================ */
    // ���� ��Ʈ���� ��ٸ��� �ڷ�ƾ
    private Coroutine waitNextPatrolCoroutine;
    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;

    /* ============== üũ�� bool Ÿ�� ================ */
    [SerializeField] bool hasTarget = false; // Ÿ������

    // ���� ������
    private Vector3 v3nextPosition;
    // navmeshHit
    private NavMeshHit hit;

    // ���� ��ǥ
    [SerializeField] Transform createPosition;


    private void OnDrawGizmos()
    {
        // ���� (��ǥ) ��ġ
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(v3nextPosition, 3);
    }

    #region OnEnable, OnDisable

    private void OnEnable()
    {
        timeForWaitingPatrol = 5f;

        createPosition = GetComponent<CreatureController>().GetCreatePosition();
        v3nextPosition = createPosition.position;
    }

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        finder = GetComponent<CreatureTargetFinder>();
    }
    private void Update()
    {
        if (GetComponent<CreatureCaster>().GetIsCasting()) return;

        finder.FindTarget();

        if(finder.IsInTrackingRange() && !finder.IsInAttackRange())
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

        Patrol();
    }

    private void Patrol()
    {
        agent.updateRotation = true;
        agent.isStopped = false;

        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // �ִϸ��̼�
        animator.SetFloat("Speed", 0.1f);

        // ������ ��ġ�� �����ߴ���
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

    public void StartTrackingBehaviour()
    {
        Debug.Log("Mover.StartTrackingBehaviour()");
        // GetComponent<CreatureActionScheduler>().StartAction(this);

        Tracking();
    }
    private void Tracking()
    {
        transform.LookAt(finder.GetTarget().transform);

        // �ӽ� Ÿ���� ������� ���� �ڷ�ƾ ����
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
        v3nextPosition = finder.GetTarget().transform.position;

        // ���� ��ǥ�� �̵�
        agent.destination = v3nextPosition;
        // Ʈ��ŷ �ӵ��� ��ȯ
        agent.speed = creature.GetTrackingSpeed();
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
        timeForWaitingPatrol = 5f;
        // �ڷ�ƾ ����
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// �����ߴ��� �Ǻ��ϴ� �޼���
    /// </summary>
    private bool IsArrive()
    {
        // ���� �����ǰ� ũ������ �Ÿ� ���
        float distanceToWaypoint = Vector3.Distance(transform.position, v3nextPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }

    /// <summary>
    /// ��Ʈ�� �ð��� �󸶳� ���������� ��� �ڷ�ƾ
    /// �ӽ�Ÿ�� (=tempTarget)�� ���� �� ����
    /// </summary>
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
                    // 1. Ÿ���� �������� ǥ��
                    // hasTarget = false;

                    // 2. ���� Ÿ���� ����
                    // finder.GetTarget() = null;

                    // 3. ���ο� ��ǥ�� ����
                    UpdatePath();

                    // ����������
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼���
    /// </summary>
    public void UpdatePath()
    {
        // ���� X, Z ��ǥ ���� - CreatePosition�� �߽�����
        // createPosition - creature.patrolRange => (���� ������ - ũ���� ��Ʈ�� ����)        
        float randomX = UnityEngine.Random.Range(createPosition.position.x - creature.GetPatrolRange(), createPosition.position.x + creature.GetPatrolRange());
        float randomZ = UnityEngine.Random.Range(createPosition.position.z - creature.GetPatrolRange(), createPosition.position.z + creature.GetPatrolRange());

        // �������� ������ ������ ����
        v3nextPosition = new Vector3(randomX, transform.position.y, randomZ);

        // ��ǥ�� �̸� ����غ��� hit�� ��ȯ
        // ((����)) �Ű����� maxDistance �κ��� �۾��� ���� ���귮�� ������ -> ���� �����÷ο�!! 
        // �̸� �����ϱ� ���� NavMash�� �� �β��� ó����... (�Ф�)
        NavMesh.SamplePosition(v3nextPosition, out hit, 10f, 1);

        // ����� ����� �� bake �� ������ �ƴϸ� x,y,z ��ǥ ���� Infinity�� ��!!!
        // Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);

        // bake �� ���� �ٱ��̸� 
        if (hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // ��ǥ�� ���� ���� 
            UpdatePath();
        }

        v3nextPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // ȸ��
        // ȸ�� ������Ʈ ����
        agent.updateRotation = false;
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        agent.destination = v3nextPosition;
        agent.speed = creature.GetPatrolSpeed();

        // Ÿ���� ������ Ÿ�� �ٶ󺸱� 
        // if (targetCharacter != null)
        // {
        //     transform.LookAt(targetCharacter.transform);
        // }

        // agent.updateRotation = true;
    }
    

    public void Cancel()
    {
        Debug.Log("Mover.Cancel()");

        // target�� ����
        // tempTarget = null;
        // trackingTargetCharacter = null;

        // agent �ʱ�ȭ
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // �ڷ�ƾ �������̸� ������ �ڷ�ƾ ����
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
            // �ð� �ʱ�ȭ
            timeForWaitingPatrol = 5f;
            // �ڷ�ƾ ����
            waitNextPatrolCoroutine = null;
        }
        if (timeLastPatrolCoroutine != null)
        {
            StopCoroutine(timeLastPatrolCoroutine);
            // �ð� �ʱ�ȭ
            timeSinceLastPatrol = 0f;
            // �ڷ�ƾ ����
            timeLastPatrolCoroutine = null;
        }
    }
}