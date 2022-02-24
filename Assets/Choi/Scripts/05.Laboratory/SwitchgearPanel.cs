using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchgearPanel : MonoBehaviour
{
    // 컴포넌트
    private Switchgear switchgear;
    private Text panelText;

    // 이 블록의 번호 - 외부(에디터)에서 설정
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // 선택되었는지
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; } set { isSelect = value; } }

    // 현재 값
    [SerializeField] int currentValue;
    public int GetCurrentValue() { return currentValue; }


    private void Awake()
    {
        switchgear = GetComponentInParent<Switchgear>();
        panelText = GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        // 선택되지 않음 설정
        isSelect = false;

        // 값은 1로 설정
        currentValue = 1;
    }
    private void Start()
    {
        // 배열 값 변경
        switchgear.SetArray(index, currentValue);
    }
    private void Update()
    {
        // 선택되었으면
        if (isSelect)
        {
            // 키를 눌러 패널 값 바꾸기
            UpdateValue();

            // text 업데이트
            UpdateText();
        }
    }

    private void UpdateValue()
    {        
        if (Input.GetKeyDown(KeyCode.W)) // 위
        {
            // 값 변경
            currentValue--;
            if (currentValue < 0) currentValue = 9;

            // 배열 값 변경
            switchgear.SetArray(index, currentValue);
        }
        else if (Input.GetKeyDown(KeyCode.S)) // 아래
        {
            // 값 변경
            currentValue++;
            if (currentValue > 9) currentValue = 0;

            // 배열 값 변경
            switchgear.SetArray(index, currentValue);
        }
    }

    private void UpdateText()
    {
        panelText.text = currentValue.ToString();
    }
}
