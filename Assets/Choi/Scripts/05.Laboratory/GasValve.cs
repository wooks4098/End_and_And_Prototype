using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    // ���� ��ġ�� �����ϴ� �迭
    private int[,] correct;
    // ��ü �迭�� �����ϴ� �迭
    private int[,] array;

    // ���(����)�� �����ϴ� �迭
    [SerializeField] GameObject[] symbols;
    public GameObject GetSymbol(int _index) { return symbols[_index]; }

    private int currentSymbol = 0; // 0~4, �� 5��

    private void Awake()
    {
        correct = new int[,] { { 0, 0, 0, 1, 0 },
                               { 0, 0, 1, 0, 0 },
                               { 0, 0, 0, 0, 1 },
                               { 0, 1, 0, 0, 0 },
                               { 1, 0, 0, 0, 0 } };
        
        array = new int[,] { { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0 } };
    }

    public void SetPosition(GameObject _obj)
    {
        GasValveSymbol sm = _obj.GetComponent<GasValveSymbol>();

        // array[sm.GetXBoard(), sm.GetYBoard()] = _obj;
    }

    public void SetPositionEmpty(int _x, int _y)
    {
        array[_x, _y] = 0;
    }

    public int GetPosition(int _x, int _y)
    {
        return array[_x, _y];
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