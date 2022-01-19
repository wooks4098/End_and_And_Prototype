using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureCaster : MonoBehaviour, ICreatureAction
{
    [SerializeField] GameObject goCastingProjector; // ĳ���õ��� ������ ǥ���� ��������


    public void StartSpellCastBehaviour()
    {
        GetComponent<CreatureActionScheduler>().StartAction(this);
        CastingBehaviour();
    }

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
