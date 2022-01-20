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

    /* ============== �ð� ================ */
    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 5f;
    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== üũ�� bool Ÿ�� ================ */
    [SerializeField] bool hasTarget = false; // Ÿ������

    /* ============== �ڷ�ƾ ================ */
    // ���� ��Ʈ���� ��ٸ��� �ڷ�ƾ
    private Coroutine waitNextPatrolCoroutine;
    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;

    /* ============== Ÿ�� ================ */
    // �ӽ� Ÿ��
    [SerializeField] CreaturePlayer tempTarget;
    // ���� Ÿ��
    [SerializeField] CreaturePlayer trackingTargetCharacter;
    public CreaturePlayer GetTargetCharacter() { return trackingTargetCharacter; }

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


    private void OnEnable()
    {
        timeForWaitingPatrol = 5f;

        v3nextPosition = createPosition.position;
    }
    private void OnDisable()
    {

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        FindTrackingTargetCharacter();

        if(IsInTrackingRange())
        {
            if (GetComponent<CreatureCaster>().GetIsCasting()) return;

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
        transform.LookAt(trackingTargetCharacter.transform);

        // �ӽ� Ÿ���� ������� ���� �ڷ�ƾ ����
        if (tempTarget == null)
        {
            if (timeLastPatrolCoroutine == null)
            {
                timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol());
            }
        }

        // �ִϸ��̼�
        GetComponent<Animator>().SetFloat("Speed", 0.6f);

        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        v3nextPosition = trackingTargetCharacter.transform.position;

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
        timeForWaitingPatrol = 2f;
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
                if (tempTarget == null)
                {
                    // 1. Ÿ���� �������� ǥ��
                    // hasTarget = false;

                    // 2. ���� Ÿ���� ����
                    trackingTargetCharacter = null;

                    // 3. ���ο� ��ǥ�� ����
                    GetComponent<CreaturePatroller>().UpdatePath();

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
                }

                // Ÿ�� ����
                trackingTargetCharacter = tempTarget;
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


    public void Cancel()
    {
        Debug.Log("Mover.Cancel()");

        // �ڷ�ƾ �������̸� ������ �ڷ�ƾ ����
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }

        timeForWaitingPatrol = 5f;

        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}
