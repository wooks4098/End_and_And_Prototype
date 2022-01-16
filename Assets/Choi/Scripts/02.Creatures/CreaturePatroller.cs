using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreaturePatroller : MonoBehaviour, ICreatureAction
{
    // ��Ʈ�� ���� �� ��� �ð�
    [SerializeField] float timeForWaitingPatrol = 5f;

    // ���������� ��Ʈ���� ���� �ð�
    [SerializeField] float timeSinceLastPatrol = 0f;

    // ���� ��Ʈ���� ��ٸ��� �ڷ�ƾ
    private Coroutine waitNextPatrolCoroutine;

    // ���� ������
    private Vector3 v3nextPosition;

    [SerializeField] Creature creatureInfo;
    private NavMeshHit hit;

    public void StartPatrolBehaviour(Transform _createPosition)
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);

        Preset(_createPosition);
        Patrol(_createPosition);
    }

    private void Preset(Transform _createPosition)
    {
        if (!GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }

        v3nextPosition = _createPosition.position;
        timeForWaitingPatrol = 5f;

        GetComponent<NavMeshAgent>().destination = v3nextPosition;
        GetComponent<NavMeshAgent>().speed = creatureInfo.patrolSpeed;
    }

    private void Patrol(Transform _createPosition)
    {
        GetComponent<NavMeshAgent>().updateRotation = true;
        GetComponent<NavMeshAgent>().isStopped = false;

        // �ִϸ��̼�
        GetComponent<Animator>().SetFloat("Speed", 0.1f);

        // ������ �����ߴ���
        if (IsArrive(v3nextPosition))
        {
            // ���������� 1. �ִϸ��̼� Idle��
            GetComponent<Animator>().SetFloat("Speed", 0.0f);
            // 2. ����
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // ������ ��Ʈ�� �ð� 0���� �ʱ�ȭ
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol(_createPosition));
            }
        }
    }

    /// <summary>
    /// ���� ��Ʈ�� ��ǥ�� ã����� �ð��� ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitNextPatrol(Transform _createPosition)
    {
        while (true)
        {
            timeForWaitingPatrol -= Time.deltaTime;

            yield return new WaitForFixedUpdate();

            if (timeForWaitingPatrol < 0f)
            {
                // ���ο� ��ǥ�� �����ϰ�
                UpdatePath(_createPosition);
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
    private bool IsArrive(Vector3 _targetPosition)
    {
        // ���� �����ǰ� ũ������ �Ÿ� ���
        float distanceToWaypoint = Vector3.Distance(transform.position, _targetPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }

    /// <summary>
    /// ��ǥ�� �����ϴ� �޼���
    /// </summary>
    private void UpdatePath(Transform _createPosition)
    {
        // ���� X, Z ��ǥ ���� - CreatePosition�� �߽�����
        // createPosition - creature.patrolRange => (���� ������ - ũ���� ��Ʈ�� ����)        
        float randomX = UnityEngine.Random.Range(_createPosition.position.x - creatureInfo.patrolRange, _createPosition.position.x + creatureInfo.patrolRange);
        float randomZ = UnityEngine.Random.Range(_createPosition.position.z - creatureInfo.patrolRange, _createPosition.position.z + creatureInfo.patrolRange);

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
            UpdatePath(_createPosition);
        }

        v3nextPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // ȸ��
        // ȸ�� ������Ʈ ����
        GetComponent<NavMeshAgent>().updateRotation = false;
        if (GetComponent<NavMeshAgent>().velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        }

        GetComponent<NavMeshAgent>().destination = v3nextPosition;
        GetComponent<NavMeshAgent>().speed = creatureInfo.patrolSpeed;

        // // Ÿ���� ������ Ÿ�� �ٶ󺸱� 
        // if (targetCharacter != null)
        // {
        //     transform.LookAt(targetCharacter.transform);
        // }
        // 
        // // agent.updateRotation = true;
    }


    public void Cancel()
    {
        // �ڷ�ƾ �������̸� ������ �ڷ�ƾ ����
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }
    }
}
