using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    // 컴포넌트
    private NavMeshAgent agent;
    private Animator animator;

    // 캐스팅동안 범위를 표시할 프로젝터
    [SerializeField] GameObject goCastingProjector; 

    // 타겟 캐릭터
    [SerializeField] CreaturePlayer targetCharacter;

    // 체크용 bool 타입 변수
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
    /// 캐스팅 행동
    /// </summary>
    private void CastingBehaviour()
    {
        isCasting = true;

        // 애니메이터
        animator.SetTrigger("Prepare Attack");        
    }

    #region call in casting animation 
    /// <summary>
    /// 캐스팅 애니메이션 실행 시 호출되는 메서드들
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
