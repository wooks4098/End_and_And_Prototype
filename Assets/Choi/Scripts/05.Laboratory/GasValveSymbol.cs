using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // 컴포넌트
    private RectTransform rectTransform;
    public GameObject goController;
    public GameObject goMovePlate;

    // array 인덱스 (보드 인덱스) - 외부(에디터)에서 설정
    [SerializeField] int xBoard = -1;
    [SerializeField] int yBoard = -1;

    // 선택되었는지
    public bool isSelect = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        // 선택되지 않음 설정
        isSelect = false;

        // 인덱스에 맞게 위치 이동
        float x = xBoard * 100;
        float y = yBoard * 100;
        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            float x = GetComponent<RectTransform>().anchoredPosition.x;
            float y = GetComponent<RectTransform>().anchoredPosition.y;

            y -= 100;

            GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
        }
    }

    public void Activate()
    {
        // 게임 컨트롤러 찾기
        goController = GameObject.Find("Gas Valve Controller");
    }

    public void SetCoord()
    {
        //
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int _x)
    {
        xBoard = _x;
    }
    public void SetYBoard(int _y)
    {
        yBoard = _y;
    }

    public void InitiateMovePlates()
    {
    }
}