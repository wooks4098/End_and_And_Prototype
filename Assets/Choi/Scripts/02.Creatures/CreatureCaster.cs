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
    /// ĳ���� �ൿ
    /// </summary>
    private void CastingBehaviour()
    {
        //isCasting = true;

        GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        GetComponent<NavMeshAgent>().isStopped = true;

        // �ִϸ�����
        GetComponent<Animator>().SetTrigger("Prepare Attack");
    }
}
