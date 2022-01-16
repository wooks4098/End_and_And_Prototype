using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
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
}
