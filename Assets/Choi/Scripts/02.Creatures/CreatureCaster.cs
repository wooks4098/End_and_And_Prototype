using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    // 컴포넌트
    private NavMeshAgent agent;
    private Animator animator;
    private CreatureHp hp;

    // 크리쳐 정보
    [SerializeField] CreatureSO creature;

    // 캐스팅동안 범위를 표시할 프로젝터
    [SerializeField] GameObject goCastingProjector; 

    /* ============== 체크용 bool 타입 ============== */
    // 캐스팅 중인가
    [SerializeField] bool isCasting = false;
    public bool GetIsCasting() { return isCasting; }

    // 캐스팅 속도 - 컨트롤러로부터 받아옴
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
    /// 캐스팅 행동
    /// </summary>
    private void CastingBehaviour()
    {
        isCasting = true;

        // 캐스팅 실행
        animator.SetTrigger("Prepare Attack");
        // 캐스팅 스피드
        castingSpeed = GetComponent<CreatureController>().GetAnimatingSpeed();
        animator.SetFloat("Animating Speed", castingSpeed);
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
        // 캐스팅이 끝나면서 타겟찾기
        GetComponent<CreatureTargetFinder_Test>().FindTarget();

        // 장판용 프로젝터를 끄고
        goCastingProjector.SetActive(false);
        // 캐스팅 중임을 false로
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
