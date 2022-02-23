using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchgear : MonoBehaviour
{
    // 정답을 저장할 배열
    private int[] correct;
    // 플레이어가 입력한 값을 관리하는 배열
    private int[] array;

    // 정답 체크
    public bool isPerfect = false;


    private void Awake()
    {
        // 0을 제외, 1~5의 값은 각 블록(문양) 인덱스에서 +1한 값
        correct = new int[] { 9, 6, 4, 1, 7 };
        array = new int[] { 0, 0, 0, 0, 0 };
    }

    public void SetArray(int _x, int _value)
    {
        array[_x] = _value;
    }    
}
