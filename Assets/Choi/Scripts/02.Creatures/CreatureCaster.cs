using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    // ������Ʈ
    private NavMeshAgent agent;
    private Animator animator;
    private CreatureHp hp;

    // ũ���� ����
    [SerializeField] CreatureSO creature;

    // ĳ���õ��� ������ ǥ���� ��������
    [SerializeField] GameObject goCastingProjector; 

    /* ============== üũ�� bool Ÿ�� ============== */
    // ĳ���� ���ΰ�
    [SerializeField] bool isCasting = false;
    public bool GetIsCasting() { return isCasting; }

    // ĳ���� �ӵ� - ��Ʈ�ѷ��κ��� �޾ƿ�
    private float castingSpeed;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hp = GetComponent<CreatureHp>();
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

        // ĳ���� ����
        animator.SetTrigger("Prepare Attack");
        // ĳ���� ���ǵ�
        castingSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", castingSpeed);
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
        GetComponent<CreatureTargetFinder_Test>().FindTarget();

        // ���ǿ� �������͸� ����
        goCastingProjector.SetActive(false);
        // ĳ���� ������ false��
        isCasting = false;

        // 
        GetComponent<CreatureController>().CanCasting = false;

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
