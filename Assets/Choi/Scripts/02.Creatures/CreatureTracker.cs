using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureTracker : MonoBehaviour, ICreatureAction
{
    // 마지막으로 패트롤한 시간을 재는 코루틴 -> 적 추격 상태를 빠져나올 때 사용
    private Coroutine timeLastPatrolCoroutine;

    // 다음 포지션
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


        // 임시 타겟이 비어있을 때만 코루틴 실행
        // if (_tempTarget == null)
        // {
        //     if (timeLastPatrolCoroutine == null)
        //     {
        //         timeLastPatrolCoroutine = StartCoroutine(TimeLastPatrol(_tempTarget));
        //     }
        // }

        // 애니메이션
        GetComponent<Animator>().SetFloat("Speed", 0.6f);

        // 다음 목표 좌표를 플레이어로 설정
        v3nextPosition = _targetCharacter.transform.position;

        // 다음 목표로 이동
        GetComponent<NavMeshAgent>().destination = v3nextPosition;
        // 트래킹 속도로 전환
        GetComponent<NavMeshAgent>().speed = creatureInfo.trackingSpeed;
    }

    /// <summary>
    /// 패트롤 시간이 얼마나 지났는지를 재는 코루틴
    /// </summary>
    /// <returns></returns>
    // private IEnumerator TimeLastPatrol(CreaturePlayer _tempTarget)
    // {
    //     while (true)
    //     {
    //         // 임시 타겟이 생기는 순간 루프를 빠져나감
    //         if (_tempTarget != null) break;
    // 
    //         // 마지막으로 패트롤한지
    //         timeSinceLastPatrol += Time.deltaTime;
    // 
    //         yield return new WaitForFixedUpdate();
    // 
    //         // 10초를 초과하면
    //         if (timeSinceLastPatrol > 10f)
    //         {
    //             // 타겟을 찾아본다
    //             // FindTargetCharacter();
    // 
    //             // 임시 타겟이 없으면
    //             if (_tempTarget == null)
    //             {
    //                 // 1. 타겟을 없음으로 표시
    //                 hasTarget = false;
    // 
    //                 // 2. 실제 타겟을 비우고
    //                 targetCharacter = null;
    // 
    //                 // 3. 새로운 좌표를 지정
    //                 UpdatePath();
    // 
    //                 // 빠져나간다
    //                 break;
    //             }
    //         }
    //     }
    // 
    //     // 다시 시간을 0으로
    //     timeSinceLastPatrol = 0f;
    //     // 코루틴 비우기
    //     timeLastPatrolCoroutine = null;
    // }

    public void Cancel()
    {
        
    }
}
