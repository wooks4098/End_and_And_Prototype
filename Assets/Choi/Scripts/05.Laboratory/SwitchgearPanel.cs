using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchgearPanel : MonoBehaviour
{
    // 컴포넌트
    private UnityEngine.UI.Outline outline;

    // 이 블록의 번호 - 외부(에디터)에서 설정
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // 선택되었는지
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; } set { isSelect = value; } }


    private void Awake()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();
    }
    private void OnEnable()
    {
        // 선택되지 않음 설정
        isSelect = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }

        // 선택되었으면
        if (isSelect)
        {
            // 키를 눌러 심볼 움직이기
            ChangeValue();
        }
    }

    private void ChangeValue()
    {
        
    }
}
