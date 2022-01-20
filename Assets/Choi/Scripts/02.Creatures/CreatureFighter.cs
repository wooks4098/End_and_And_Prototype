using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    Animator animator;
    NavMeshAgent agent;
        
    // ũ���� ����
    [SerializeField] CreatureSO creature;

    /* ============== Ÿ�� ================ */
    // �ӽ� Ÿ��
    [SerializeField] CreaturePlayer tempTarget;
    // ���� Ÿ��
    [SerializeField] CreaturePlayer targetCharacter;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();        
    }

    public void StartAttackBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
    }



    /// <summary>
    /// �÷��̾�� �Ÿ� ��� (���� ����)
    /// </summary>
    public bool IsInAttackRange()
    {
        if (GetComponent<CreatureMover>().GetTargetCharacter() == null) return false;

        // �÷��̾�� ũ��ó�� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(GetComponent<CreatureMover>().GetTargetCharacter().transform.position, transform.position);
        //Debug.Log(distanceToPlayer);

        // ���� ���� attack �������� ������ true
        return distanceToPlayer < creature.GetAttackRange();
    }

    public void Cancel()
    {
        // StopAttack();
        // target = null;
        GetComponent<CreatureTracker>().Cancel();
    }




}
