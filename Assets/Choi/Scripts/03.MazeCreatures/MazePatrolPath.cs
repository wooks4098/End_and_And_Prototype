using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڽ����κ��� ��ǥ(������Ʈ)�� �޾ƿ��� ���
/// </summary>
public class MazePatrolPath : MonoBehaviour
{
    // �ڽ��� ����
    private int count;

    // ������ ���� ������ stack
    private Stack<Transform> wayPoints;

    // �ݴ�� ���� ������ (����ȸ ������) üũ�ϴ� �÷���
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
        // stack�� ���� ������� ������ �̵�
        if (wayPoints.Count == 0) isReverse = false;
        // stack�� �ڽ� ������ŭ á���� ������ �̵�
        else if (wayPoints.Count >= (count - 1)) isReverse = true;

        // �������̸�
        if (!isReverse)
        {
            // ���� ä���
            wayPoints.Push(transform.GetChild(i));
            
            return i + 1;
        }
        // �������̸�
        else 
        {
            // ���� 1���� ����
            wayPoints.Pop();

            return i - 1;
        }
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }
}