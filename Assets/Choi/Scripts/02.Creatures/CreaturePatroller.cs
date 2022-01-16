using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreaturePatroller : MonoBehaviour, ICreatureAction
{
    // 패트롤 끝난 후 대기 시간
    [SerializeField] float timeForWaitingPatrol = 5f;

    // 마지막으로 패트롤을 멈춘 시간
    [SerializeField] float timeSinceLastPatrol = 0f;

    // 다음 패트롤을 기다리는 코루틴
    private Coroutine waitNextPatrolCoroutine;

    // 다음 포지션
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

        // 애니메이션
        GetComponent<Animator>().SetFloat("Speed", 0.1f);

        // 지정한 도착했는지
        if (IsArrive(v3nextPosition))
        {
            // 도착했으면 1. 애니메이션 Idle로
            GetComponent<Animator>().SetFloat("Speed", 0.0f);
            // 2. 멈춤
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            // 마지막 패트롤 시간 0으로 초기화
            timeSinceLastPatrol = 0f;

            if (waitNextPatrolCoroutine == null)
            {
                waitNextPatrolCoroutine = StartCoroutine(WaitNextPatrol(_createPosition));
            }
        }
    }

    /// <summary>
    /// 다음 패트롤 좌표를 찾기까지 시간을 재는 코루틴
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
                // 새로운 좌표를 지정하고
                UpdatePath(_createPosition);
                // 빠져나간다
                break;
            }
        }
        // 시간을 5로
        timeForWaitingPatrol = 2f;
        // 코루틴 비우기
        waitNextPatrolCoroutine = null;
    }

    /// <summary>
    /// 도착했는지 판별하는 메서드
    /// </summary>
    private bool IsArrive(Vector3 _targetPosition)
    {
        // 다음 포지션과 크리쳐의 거리 계산
        float distanceToWaypoint = Vector3.Distance(transform.position, _targetPosition);
        // Debug.Log(distanceToWaypoint);

        return distanceToWaypoint <= 2.6f;
    }

    /// <summary>
    /// 좌표를 갱신하는 메서드
    /// </summary>
    private void UpdatePath(Transform _createPosition)
    {
        // 랜덤 X, Z 좌표 생성 - CreatePosition을 중심으로
        // createPosition - creature.patrolRange => (생성 포지션 - 크리쳐 패트롤 범위)        
        float randomX = UnityEngine.Random.Range(_createPosition.position.x - creatureInfo.patrolRange, _createPosition.position.x + creatureInfo.patrolRange);
        float randomZ = UnityEngine.Random.Range(_createPosition.position.z - creatureInfo.patrolRange, _createPosition.position.z + creatureInfo.patrolRange);

        // 다음으로 움직일 포지션 지정
        v3nextPosition = new Vector3(randomX, transform.position.y, randomZ);

        // 좌표를 미리 계산해보고 hit로 반환
        // ((주의)) 매개변수 maxDistance 부분이 작아질 수록 연산량이 많아짐 -> 스택 오버플로우!! 
        // 이를 방지하기 위해 NavMash를 좀 두껍게 처리함... (ㅠㅠ)
        NavMesh.SamplePosition(v3nextPosition, out hit, 10f, 1);

        // 디버그 찍었을 때 bake 된 영역이 아니면 x,y,z 좌표 전부 Infinity가 뜸!!!
        // Debug.Log("Hit = " + hit + " myNavHit.position = " + hit.position + " target = " + targetPosition);

        // bake 된 영역 바깥이면 
        if (hit.position.x == Mathf.Infinity || hit.position.z == Mathf.Infinity)
        {
            // 좌표를 새로 갱신 
            UpdatePath(_createPosition);
        }

        v3nextPosition = hit.position;
        // Debug.DrawLine(transform.position, targetPosition, Color.white, Mathf.Infinity);

        // 회전
        // 회전 업데이트 멈춤
        GetComponent<NavMeshAgent>().updateRotation = false;
        if (GetComponent<NavMeshAgent>().velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        }

        GetComponent<NavMeshAgent>().destination = v3nextPosition;
        GetComponent<NavMeshAgent>().speed = creatureInfo.patrolSpeed;

        // // 타겟이 있으면 타겟 바라보기 
        // if (targetCharacter != null)
        // {
        //     transform.LookAt(targetCharacter.transform);
        // }
        // 
        // // agent.updateRotation = true;
    }


    public void Cancel()
    {
        // 코루틴 진행중이면 강제로 코루틴 멈춤
        if (waitNextPatrolCoroutine != null)
        {
            StopCoroutine(waitNextPatrolCoroutine);
        }
    }
}
