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
    [SerializeField] CreaturePlayer target;
    public CreaturePlayer GetTarget() { return target; }


    private void Update()
    {
        

    }

    /// <summary>
    /// 타겟을 찾는다
    /// </summary>
    private void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());


    }
}
