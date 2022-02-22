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

    GameObject reference = null;

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

        // 현재 블록(문양) 초기화
        goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

        // 좌표 초기화
        currentXPos = goCurrentSymbol.anchoredPosition.x;
        currentYPos = goCurrentSymbol.anchoredPosition.y;

        rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            symbolIndex++;
            if(symbolIndex > 4)
            {
                symbolIndex = 0;
            }
            // 현재 블록(문양) 초기화
            goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

            // 좌표 초기화
            currentXPos = goCurrentSymbol.anchoredPosition.x;
            currentYPos = goCurrentSymbol.anchoredPosition.y;

            rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            symbolIndex--;
            if(symbolIndex < 0)
            {
                symbolIndex = 4;
            }
            // 현재 블록(문양) 초기화
            goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

            // 좌표 초기화
            currentXPos = goCurrentSymbol.anchoredPosition.x;
            currentYPos = goCurrentSymbol.anchoredPosition.y;

            rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
        }
    }
    
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
       

    public void SetPosition(int _xIndex, int _yIndex)
    {
        // 블록 위치로 따라서 이동
        float x = _xIndex * 100;
        float y = _yIndex * 100 * -1;

        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }

    public void SetReference(GameObject _obj)
    {
        reference = _obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
