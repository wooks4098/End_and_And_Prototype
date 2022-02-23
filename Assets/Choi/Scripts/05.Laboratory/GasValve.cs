using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    // 정답 위치를 저장하는 배열
    private int[,] correct;
    // 전체 배열을 관리하는 배열
    private int[,] array;
    public int GetArray(int _x, int _y)  { return array[_x, _y]; }

    // 블록(문양)을 저장하는 배열
    [SerializeField] GameObject[] symbols;
    public GameObject GetSymbol(int _index) { return symbols[_index]; }

    // 정답 체크
    public bool isPerfect = false;


    private void Awake()
    {
        // 0을 제외, 1~5의 값은 각 블록(문양) 인덱스에서 +1한 값
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
        // 해당 칸이 차있음을 표시 = 각 블록(문양)의 인덱스 +1 한 값
        array[_x, _y] = _index + 1;

        // 정답인지 체크
        isPerfect = CheckArray();

        Debug.Log("CHECK: " + isPerfect);
    }

    public void SetArrayEmpty(int _x, int _y)
    {
        // 해당 칸이 비었음을 표시 = 0
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
    /// 이 칸이 비었는지
    /// </summary>
    public bool IsArrayEmpty(int _x, int _y)
    {
        // 비었으면 true
        if (array[_x, _y] == 0) return true;

        // 아니면 false
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