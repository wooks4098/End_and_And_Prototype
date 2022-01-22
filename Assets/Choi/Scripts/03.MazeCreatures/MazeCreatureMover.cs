using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCreatureMover : MonoBehaviour, ICreatureAction
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    // 컴포넌트
    private Animator animator;
    private NavMeshAgent agent;

    /* ============== 시간 ================ */
    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 5f;
    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    /* ============== 코루틴 ================ */
    // 다음 패트롤을 기다리는 코루틴
    private Coroutine waitNextPatrolCoroutine;
    // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용
    private Coroutine timeLastPatrolCoroutine;

    /* ============== 체크용 bool 타입 ================ */
    [SerializeField] bool hasTarget = false; // 타겟유무

    /* ============== 타겟 ================ */
    // 실제 타겟
    [SerializeField] CreaturePlayer trackingTargetCharacter;
    public CreaturePlayer GetTargetCharacter() { return trackingTargetCharacter; }

    // 다음 포지션
    private Vector3 v3nextPosition;


    #region DrawGizmos
    private void OnDrawGizmos()
    {
        // 다음 (목표) 위치
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
