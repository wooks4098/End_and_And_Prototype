using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ���� ã�� ���� ���� ������Ʈ
/// </summary>
public class CreatureTargetFinder_Test : MonoBehaviour
{
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // aggro
    private CreatureAggro aggro;

    /* ============== �׽�Ʈ�� Ÿ�� ���� ================ */
    [SerializeField] CreaturePlayer testTarget;
    public CreaturePlayer GetTestTarget() { return testTarget; }


    private void Awake()
    {
        aggro = GetComponent<CreatureAggro>();
    }

    /// <summary>
    /// �÷��̾���� �Ÿ� ��� (Ʈ��ŷ ����)
    /// </summary>
    public bool IsInTrackingRange()
    {
        if (testTarget == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(testTarget.transform.position, transform.position);
        // Debug.Log(distanceToPlayer);

        // ���� ���� tracking �������� ������ true
        return distanceToPlayer < creature.GetTrackingRange();
    }

    /// <summary>
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (testTarget == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(testTarget.transform.position, transform.position);
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

        // �÷��̾� 1, 2 ���� ��
        var value = Mathf.Max(aggro.Player1Point, aggro.Player2Point);
        if (value > aggro.Player1Point)
        {

        }

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
            if (activeCollider.GetComponent<CreaturePlayer>() != null
                && !activeCollider.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {
                

                testTarget = activeCollider.GetComponent<CreaturePlayer>();
            }
        }

        if (testTarget != null && testTarget.GetComponent<CreaturePlayer>().GetIsDead())
        {
            testTarget = null;
        }
    }
}