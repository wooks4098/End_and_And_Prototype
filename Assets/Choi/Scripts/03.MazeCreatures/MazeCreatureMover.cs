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

    /* ============== Ÿ�� ================ */
    // ���� Ÿ��
    [SerializeField] CreaturePlayer trackingTargetCharacter;
    public CreaturePlayer GetTargetCharacter() { return trackingTargetCharacter; }

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


    public void StartPatrolBehaviour()
    {
        
    }

    public void Cancel()
    {

    }
}
