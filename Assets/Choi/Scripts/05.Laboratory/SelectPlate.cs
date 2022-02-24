using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlate : MonoBehaviour
{
    // 컴포넌트
    private Switchgear switchgear;
    private RectTransform rectTransform;
    private Image image;

    // 패널 인덱스
    private int panelIndex;
    // 현재 블록(문양)
    [SerializeField] RectTransform goCurrentPanel;
    // 현재 좌표를 저장할 변수
    private float currentXPos;
    private float currentYPos;

    // false: 패널 선택 가능, true: 패널 값 변경 가능
    public bool isConfirm = false;


    private void Awake()
    {
        switchgear = GetComponentInParent<Switchgear>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void Start()
    {
        // 인덱스 초기화
        panelIndex = 0;

        // 좌표(위치) 설정
        SetPosition();
    }

    /// <summary>
    /// 좌표 설정 - 문양을 중심으로
    /// </summary>
    private void SetPosition()
    {
        // 현재 패널 초기화
        goCurrentPanel = switchgear.GetPanel(panelIndex).GetComponent<RectTransform>();

        // 좌표 초기화
        currentXPos = goCurrentPanel.anchoredPosition.x;
        currentYPos = goCurrentPanel.anchoredPosition.y;

        // 좌표 설정
        rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
    }

    private void Update()
    {
        // 선택(확인) 키 입력
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isConfirm)
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
            goCurrentPanel.GetComponent<SwitchgearPanel>().IsSelect = false;

            SelectPanelIndex();
        }
        else if (isConfirm)
        {
            // 색상 변경
            ChangeColor();

            // 현재 심볼이 선택되었음을 표시
            goCurrentPanel.GetComponent<SwitchgearPanel>().IsSelect = true;

            // 심볼에 따라 좌표 업데이트
            // 심볼은 isSelect = true;이면 키를 눌러 움직일 수 있다.
            SetPosition();
        }
    }

    private void SelectPanelIndex()
    {
        // 방향키
        if (Input.GetKeyDown(KeyCode.D))
        {
            // 인덱스 증가
            panelIndex++;
            if (panelIndex > 4) panelIndex = 0;

            // 좌표(위치) 설정
            SetPosition();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // 인덱스 감소
            panelIndex--;
            if (panelIndex < 0) panelIndex = 4;

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
