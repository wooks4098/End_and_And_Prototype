using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    // 컴포넌트
    private GasValve gasValve;
    private RectTransform rectTransform;
    private Image image;

    // 블록(문양) 인덱스
    private int symbolIndex;
    // 현재 블록(문양)
    [SerializeField] RectTransform goCurrentSymbol;
    // 현재 좌표를 저장할 변수
    private float currentXPos;
    private float currentYPos;

    // false: 블록 선택 가능, true: 블록 위치 이동 가능
    public bool isConfirm = false;


    private void Awake()
    {
        gasValve = GetComponentInParent<GasValve>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void Start()
    {
        // 인덱스 초기화
        symbolIndex = 0;

        // 좌표(위치) 설정
        SetPosition();
    }

    /// <summary>
    /// 좌표 설정 - 문양을 중심으로
    /// </summary>
    private void SetPosition()
    {
        // 현재 블록(문양) 초기화
        goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

        // 좌표 초기화
        currentXPos = goCurrentSymbol.anchoredPosition.x;
        currentYPos = goCurrentSymbol.anchoredPosition.y;

        // 좌표 설정
        rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
    }

    private void Update()
    {
        // 선택(확인) 키 입력
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(!isConfirm)
            {
                isConfirm = true;
            }
            else if (isConfirm)
            {
                isConfirm = false;
            }
        }

        if (!isConfirm)
        {
            // 색상 변경
            ChangeColor();

            // 현재 심볼이 선택해제되었음을 표시
            goCurrentSymbol.GetComponent<GasValveSymbol>().IsSelect = false;

            SelectSymbolIndex();
        }
        else if(isConfirm)
        {
            // 색상 변경
            ChangeColor();

            // 현재 심볼이 선택되었음을 표시
            goCurrentSymbol.GetComponent<GasValveSymbol>().IsSelect = true;

            // 심볼에 따라 좌표 업데이트
            // 심볼은 isSelect = true;이면 키를 눌러 움직일 수 있다.
            SetPosition();
        }
    }

    private void SelectSymbolIndex()
    {
        // 방향키
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            // 인덱스 증가
            symbolIndex++;
            if (symbolIndex > 4) symbolIndex = 0;

            // 좌표(위치) 설정
            SetPosition();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            // 인덱스 감소
            symbolIndex--;
            if (symbolIndex < 0) symbolIndex = 4;

            // 좌표(위치) 설정
            SetPosition();
        }
    }

    /// <summary>
    /// 색상 변경
    /// </summary>
    private void ChangeColor()
    {
        // 색상 변경
        if (isConfirm)
        {
            // Change to red
            image.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            image.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
