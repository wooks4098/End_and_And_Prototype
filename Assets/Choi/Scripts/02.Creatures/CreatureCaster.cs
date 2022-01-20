using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    private NavMeshAgent agent;
    private Animator animator;

    // ĳ���õ��� ������ ǥ���� ��������
    [SerializeField] GameObject goCastingProjector; 

    // Ÿ�� ĳ����
    [SerializeField] CreaturePlayer targetCharacter;

    // üũ�� bool Ÿ�� ����
    private bool isCasting = false;
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
        goCastingProjector.SetActive(false);

        isCasting = false;

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
