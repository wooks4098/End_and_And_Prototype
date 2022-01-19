using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureTracker : MonoBehaviour, ICreatureAction
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ������Ʈ
    private Animator animator;
    private NavMeshAgent agent;

    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;

    // ���� ������
    private Vector3 v3nextPosition;

    [SerializeField] CreaturePlayer tempTarget;
    [SerializeField] CreaturePlayer targetCharacter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartTrackingBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);

        Tracking();
    }

    private void Tracking()
    {
        transform.LookAt(targetCharacter.transform);

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
        v3nextPosition = targetCharacter.transform.position;

        // ���� ��ǥ�� �̵�
        agent.destination = v3nextPosition;
        // Ʈ��ŷ �ӵ��� ��ȯ
        agent.speed = creature.GetTrackingSpeed();
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
                    targetCharacter = null;

                    // 3. ���ο� ��ǥ�� ����
                    GetComponent<CreaturePatroller>().UpdatePath();

                    // ����������
                    break;
                }
            }
        }
    }

    public void Cancel()
    {
        
    }
}
