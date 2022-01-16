using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureTracker : MonoBehaviour, ICreatureAction
{
    // ���������� ��Ʈ���� �ð��� ��� �ڷ�ƾ -> �� �߰� ���¸� �������� �� ���
    private Coroutine timeLastPatrolCoroutine;

    // ���� ������
    private Vector3 v3nextPosition;

    private Creature creatureInfo;


    public void StartTrackingBehaviour(CreaturePlayer _tempTarget, CreaturePlayer _targetCharacter)
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
        Tracking(_tempTarget, _targetCharacter);
    }

    private void Tracking(CreaturePlayer _tempTarget, CreaturePlayer _targetCharacter)
    {
        transform.LookAt(_targetCharacter.transform);


        // �ӽ� Ÿ���� ������� ���� �ڷ�ƾ ����
        // if (_tempTarget == null)
        // {
        //     if (timeLastPatrolCoroutine == null)
        //     {
        //         timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol(_tempTarget));
        //     }
        // }

        // �ִϸ��̼�
        GetComponent<Animator>().SetFloat("Speed", 0.6f);

        // ���� ��ǥ ��ǥ�� �÷��̾�� ����
        v3nextPosition = _targetCharacter.transform.position;

        // ���� ��ǥ�� �̵�
        GetComponent<NavMeshAgent>().destination = v3nextPosition;
        // Ʈ��ŷ �ӵ��� ��ȯ
        GetComponent<NavMeshAgent>().speed = creatureInfo.trackingSpeed;
    }

    /// <summary>
    /// ��Ʈ�� �ð��� �󸶳� ���������� ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    // private IEnumerator TimeLastPatrol(CreaturePlayer _tempTarget)
    // {
    //     while (true)
    //     {
    //         // �ӽ� Ÿ���� ����� ���� ������ ��������
    //         if (_tempTarget != null) break;
    // 
    //         // ���������� ��Ʈ������
    //         timeSinceLastPatrol += Time.deltaTime;
    // 
    //         yield return new WaitForFixedUpdate();
    // 
    //         // 10�ʸ� �ʰ��ϸ�
    //         if (timeSinceLastPatrol > 10f)
    //         {
    //             // Ÿ���� ã�ƺ���
    //             // FindTargetCharacter();
    // 
    //             // �ӽ� Ÿ���� ������
    //             if (_tempTarget == null)
    //             {
    //                 // 1. Ÿ���� �������� ǥ��
    //                 hasTarget = false;
    // 
    //                 // 2. ���� Ÿ���� ����
    //                 targetCharacter = null;
    // 
    //                 // 3. ���ο� ��ǥ�� ����
    //                 UpdatePath();
    // 
    //                 // ����������
    //                 break;
    //             }
    //         }
    //     }
    // 
    //     // �ٽ� �ð��� 0����
    //     timeSinceLastPatrol = 0f;
    //     // �ڷ�ƾ ����
    //     timeLastPatrolCoroutine = null;
    // }

    public void Cancel()
    {
        
    }
}
