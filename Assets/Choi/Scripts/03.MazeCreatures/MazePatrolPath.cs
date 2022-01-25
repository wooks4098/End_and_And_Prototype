using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자식으로부터 좌표(오브젝트)를 받아오고 계산
/// </summary>
public class MazePatrolPath : MonoBehaviour
{
    public int GetNextIndex(int i)
    {
        if (i + 1 == transform.childCount)
        {
            return 0;
        }
        return i + 1;
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }
}