using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ũ������ �������� �����ϴ� Ŭ����
/// </summary>
public class CreatureMove : MonoBehaviour
{
    public Creature creature;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
