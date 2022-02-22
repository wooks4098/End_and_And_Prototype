using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // 컴포넌트
    private RectTransform rectTransform;
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
    private void OnEnable()
    {
        // 선택되지 않음 설정
        isSelect = false;

        // 인덱스에 맞게 위치 이동
        // 배열을 맞추기 위해 y좌표에 -1 해줌
        float x = xBoard * 100;
        float y = yBoard * 100 * -1;

        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }
    private void Start()
    {
    }

        
    /// <param name="_num"> 양수 및 음수 전환을 위해 사용하는 파라미터 </param>
    private void SetPosition()
    {
        float x = xBoard * 100;
        float y = yBoard * 100 * -1;

        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }

    private void Update()
    {


        if (isSelect)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                yBoard++;
                SetPosition();
            }
        }        
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