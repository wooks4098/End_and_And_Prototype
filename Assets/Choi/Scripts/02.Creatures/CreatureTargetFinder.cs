using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟을 찾기 위해 만든 컴포넌트
/// </summary>
public class CreatureTargetFinder : MonoBehaviour
{
    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    /* ============== 타겟 관리 ================ */
    [SerializeField] PlayerController target;
    public PlayerController GetTarget() { return target; }

    /* ============== 테스트용 타겟 관리 ================ */
    [SerializeField] CreaturePlayer testTarget;
    public CreaturePlayer GetTestTarget() { return testTarget; }



    /// <summary>
    /// 플레이어와의 거리 계산 (트래킹 범위)
    /// </summary>
    public bool IsInTrackingRange()
    {
        if (target == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // 비교한 값이 tracking 범위보다 적으면 true
        return distanceToPlayer < creature.GetTrackingRange();
    }

    /// <summary>
    /// 플레이어와 거리 계산 (공격 범위)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (target == null) return false;

        // 플레이어와 크리처의 거리 계산
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // 비교한 값이 attack 범위보다 적으면 true
        return distanceToPlayer < creature.GetAttackRange();
    }

    /// <summary>
    /// 타겟을 찾는다
    /// </summary>
    public void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());

        foreach (var activeCollider in hitCollider)
        {
            // 1. 플레이어 관련 컴포넌트를 가지고 있고 2. 죽지않았고 3. 활성화 되어있는 것
            if (activeCollider.GetComponent<PlayerController>() != null
                && activeCollider.GetComponent<PlayerController>().GetPlayerState() != PlayerState.Crawl
                && activeCollider.gameObject.activeSelf)
            {
                target = activeCollider.GetComponent<PlayerController>();
            }
        }

        if (target!= null && target.GetComponent<PlayerController>().GetPlayerState() == PlayerState.Crawl)
        {
            target = null;
        }
    }
}
