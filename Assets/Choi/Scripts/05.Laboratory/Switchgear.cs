using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchgear : MonoBehaviour
{
    // 정답을 저장할 배열
    private int[] correct;
    // 플레이어가 입력한 값을 관리하는 배열
    private int[] array;
    public int GetArray(int _x) { return array[_x]; }

    // 블록(문양)을 저장하는 배열
    [SerializeField] GameObject[] panels;
    public GameObject GetPanel(int _index) { return panels[_index]; }

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
        // 전달 받은 값 변경
        array[_x] = _value;

        // 정답인지 체크
        isPerfect = CheckArray();

        Debug.Log("CHECK: " + isPerfect);
    }
    private bool CheckArray()
    {
        // 두 배열의 값 비교
        if (Enumerable.SequenceEqual(array, correct)) return true;
        return false;
    }
    }
