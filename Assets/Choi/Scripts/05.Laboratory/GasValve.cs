using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    // ���� ��ġ�� �����ϴ� �迭
    private int[,] correct;
    // ��ü �迭�� �����ϴ� �迭
    private int[,] array;
    public int GetArray(int _x, int _y)  { return array[_x, _y]; }

    // ���(����)�� �����ϴ� �迭
    [SerializeField] GameObject[] symbols;
    public GameObject GetSymbol(int _index) { return symbols[_index]; }

    // ���� üũ
    public bool isPerfect = false;


    private void Awake()
    {
        // 0�� ����, 1~5�� ���� �� ���(����) �ε������� +1�� ��
        correct = new int[,] { { 0, 0, 0, 5, 0 },
                               { 0, 0, 1, 0, 0 },
                               { 0, 0, 0, 0, 4 },
                               { 0, 3, 0, 0, 0 },
                               { 2, 0, 0, 0, 0 } };
        
        array = new int[,] { { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 } };
    }


    public void SetArray(int _x, int _y, int _index)
    {
        // �ش� ĭ�� �������� ǥ�� = �� ���(����)�� �ε��� +1 �� ��
        array[_x, _y] = _index + 1;

        // �������� üũ
        isPerfect = CheckArray();

        Debug.Log("CHECK: " + isPerfect);
    }

    public void SetArrayEmpty(int _x, int _y)
    {
        // �ش� ĭ�� ������� ǥ�� = 0
        array[_x, _y] = 0;
    }

    private bool CheckArray()
    {
        if(array.Equals(correct))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// �� ĭ�� �������
    /// </summary>
    public bool IsArrayEmpty(int _x, int _y)
    {
        // ������� true
        if (array[_x, _y] == 0) return true;

        // �ƴϸ� false
        return false;
    }

    public bool PositionOnBoard(int _x, int _y)
    {
        if( _x < 0 || _y < 0 || _x >= array.GetLength(0) || _y >= array.GetLength(1))
        {
            return false;
        }

        return true;
    }
}