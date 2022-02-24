using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchgear : MonoBehaviour
{
    // ������ ������ �迭
    private int[] correct;
    // �÷��̾ �Է��� ���� �����ϴ� �迭
    private int[] array;
    public int GetArray(int _x) { return array[_x]; }

    // ���(����)�� �����ϴ� �迭
    [SerializeField] GameObject[] panels;
    public GameObject GetPanel(int _index) { return panels[_index]; }

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
        // ���� ���� �� ����
        array[_x] = _value;

        // �������� üũ
        isPerfect = CheckArray();

        Debug.Log("CHECK: " + isPerfect);
    }
    private bool CheckArray()
    {
        // �� �迭�� �� ��
        if (Enumerable.SequenceEqual(array, correct)) return true;
        return false;
    }
    }
