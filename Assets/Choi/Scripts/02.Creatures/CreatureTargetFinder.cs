using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ���� ã�� ���� ���� ������Ʈ
/// </summary>
public class CreatureTargetFinder : MonoBehaviour
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    /* ============== Ÿ�� ���� ================ */
    [SerializeField] CreaturePlayer target;
    public CreaturePlayer GetTarget() { return target; }


    private void Update()
    {
        

    }

    /// <summary>
    /// Ÿ���� ã�´�
    /// </summary>
    private void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());


    }
}
