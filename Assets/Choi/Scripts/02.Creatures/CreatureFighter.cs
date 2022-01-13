using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureFighter : MonoBehaviour, ICreatureAction
{
    Animator animator;
    NavMeshAgent agent;

    [SerializeField] GameObject goCastingProjector; // ĳ���õ��� ������ ǥ���� ��������

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Cancel()
    {
        
    }

    public void OnCastingProjector()
    {
        goCastingProjector.SetActive(true);
    }

    public void OffCastingProjector()
    {
        goCastingProjector.SetActive(false);
    }
}
