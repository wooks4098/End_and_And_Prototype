using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchgear : MonoBehaviour
{
    // ������ ������ �迭
    private int[] correct;
    // �÷��̾ �Է��� ���� �����ϴ� �迭
    private int[] array;

    // ���� üũ
    public bool isPerfect = false;


    private void Awake()
    {
        // 0�� ����, 1~5�� ���� �� ���(����) �ε������� +1�� ��
        correct = new int[] { 9, 6, 4, 1, 7 };
        array = new int[] { 0, 0, 0, 0, 0 };
    }

    public void SetArray(int _x, int _value)
    {
        array[_x] = _value;
    }    
}
