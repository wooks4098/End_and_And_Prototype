using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    private NavMeshAgent agent;
    private Animator animator;

    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ĳ���õ��� ������ ǥ���� ��������
    [SerializeField] GameObject goCastingProjector; 

    // Ÿ�� ĳ����
    [SerializeField] List<CreaturePlayer> targetCharacter;

    /* ============== üũ�� bool Ÿ�� ============== */
    // ĳ���� ���ΰ�
    [SerializeField] bool isCasting = false;
    public bool GetIsCasting() { return isCasting; }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }   

    public void StartSpellCastBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
        CastingBehaviour();
    }

    /// <summary>
    /// ĳ���� �ൿ
    /// </summary>
    private void CastingBehaviour()
    {
        isCasting = true;

        // �ִϸ�����
        animator.SetTrigger("Prepare Attack");
    }

    /// <summary>
    ///  ĳ���õ��� Ÿ�� ĳ���� ã��
    /// </summary>
    private void FindTargetCharacterWhileCasting()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, creature.GetAttackRange());

        //if(hitCollider.Length != 0)
        //{
        //    Debug.Log("���� ã�ҽ��ϴ�!");
        //}

        foreach (var activeCollider in hitCollider)
        {
            // 1. �÷��̾� ���� ������Ʈ�� ������ �ְ� 2. �����ʾҰ� 3. Ȱ��ȭ �Ǿ��ִ� ��
            if (activeCollider.gameObject.GetComponent<CreaturePlayer>() != null
                && !activeCollider.gameObject.GetComponent<CreaturePlayer>().GetIsDead()
                && activeCollider.gameObject.activeSelf)
            {
                // Ÿ�� ����
                targetCharacter.Add(activeCollider.GetComponent<CreaturePlayer>());
            }
        }
    }


    #region call in casting animation 
    /// <summary>
    /// ĳ���� �ִϸ��̼� ���� �� ȣ��Ǵ� �޼����
    /// </summary>
    public void OnCastingProjector()
    {
        //animator.SetTrigger("Run Attack");
        goCastingProjector.SetActive(true);
    }
    public void OffCastingProjector()
    {
        // ĳ������ �����鼭 Ÿ��ã��
        FindTargetCharacterWhileCasting();

        // ���ǿ� �������͸� ����
        goCastingProjector.SetActive(false);
        // ĳ���� ������ false��
        isCasting = false;

        // ������ �� �ִٰ� ǥ��
        GetComponent<CreatureController>().CanAttack = true;

        // animator.ResetTrigger("Prepare Attack");
        // GetComponent<CreatureMovement>().CanAttack = true;
        // GetComponent<CreatureMovement>().IsCasting = false;
    }
    #endregion


    public void Cancel()
    {
        animator.ResetTrigger("Prepare Attack");
    }
}
