using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    // 정답 위치를 저장하는 배열
    private int[,] correct;
    // 전체 배열을 관리하는 배열
    private int[,] array;

    // 블록(문양)을 저장하는 배열
    [SerializeField] GameObject[] symbols;
    public GameObject GetSymbol(int _index) { return symbols[_index]; }

    private int currentSymbol = 0; // 0~4, 총 5개

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