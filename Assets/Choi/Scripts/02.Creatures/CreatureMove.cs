using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 크리쳐의 움직임을 관리하는 클래스
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
