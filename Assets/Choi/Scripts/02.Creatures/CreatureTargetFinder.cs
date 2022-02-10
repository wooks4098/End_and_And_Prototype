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
    [SerializeField] PlayerController target;
    public PlayerController GetTarget() { return target; }

    /* ============== �׽�Ʈ�� Ÿ�� ���� ================ */
    [SerializeField] CreaturePlayer testTarget;
    public CreaturePlayer GetTestTarget() { return testTarget; }



    /// <summary>
    /// �÷��̾���� �Ÿ� ��� (Ʈ��ŷ ����)
    /// </summary>
    public bool IsInTrackingRange()
    {
        if (target == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.GetTrackingRange();
    }

    /// <summary>
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (target == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(target.transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� attack �������� ������ true
        return distanceToPlayer < creature.GetAttackRange();
    }

    /// <summary>
    /// Ÿ���� ã�´�
    /// </summary>
    public void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetTrackingRange());

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
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
