using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자식으로부터 좌표(오브젝트)를 받아오고 계산
/// </summary>
public class MazePatrolPath : MonoBehaviour
{
    // 자식의 개수
    private int count;

    // 지나온 길을 저장할 stack
    private Stack<Transform> wayPoints;

    // 반대로 가는 중인지 (역순회 중인지) 체크하는 플래그
    private bool isReverse; 


    private void Awake()
    {
        wayPoints = new Stack<Transform>();
    }
    private void Start()
    {
        count = transform.childCount;
        isReverse = false;
    }

    public int GetNextIndex(int i)
    {
        // stack이 전부 비었으면 순방향 이동
        if (wayPoints.Count == 0) isReverse = false;
        // stack이 자식 개수만큼 찼으면 역방향 이동
        else if (wayPoints.Count >= (count - 1)) isReverse = true;

        // 순방향이면
        if (!isReverse)
        {
            // 값을 채운다
            wayPoints.Push(transform.GetChild(i));
            
            return i + 1;
        }
        // 역방향이면
        else 
        {
            // 값을 1개씩 뺀다
            wayPoints.Pop();

            return i - 1;
        }
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }
}