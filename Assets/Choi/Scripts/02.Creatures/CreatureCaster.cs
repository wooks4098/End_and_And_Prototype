using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    [SerializeField] GameObject goCastingProjector; // 캐스팅동안 범위를 표시할 프로젝터


    public void StartSpellCastBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
        CastingBehaviour();
    }

    public void Cancel()
    {
        
    }


    /// <summary>
    /// 캐스팅 행동
    /// </summary>
    private void CastingBehaviour()
    {
        //isCasting = true;

        GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        GetComponent<NavMeshAgent>().isStopped = true;

        // 애니메이터
        GetComponent<Animator>().SetTrigger("Prepare Attack");
    }



    public void OnCastingProjector()
    {
        //animator.SetTrigger("Run Attack");
        goCastingProjector.SetActive(true);
    }

    public void OffCastingProjector()
    {
        goCastingProjector.SetActive(false);

        // animator.ResetTrigger("Prepare Attack");
        // GetComponent<CreatureMovement>().CanAttack = true;
        // GetComponent<CreatureMovement>().IsCasting = false;
    }
}
