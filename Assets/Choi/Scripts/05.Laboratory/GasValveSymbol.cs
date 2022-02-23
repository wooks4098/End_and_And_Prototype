using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // 컴포넌트
    private RectTransform rectTransform;
    [SerializeField] GasValve gasValve;

    // 이 블록(문양)의 번호 - 외부(에디터)에서 설정
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // array 인덱스 (보드 인덱스) - 외부(에디터)에서 설정
    [SerializeField] int xBoard = -1;
    [SerializeField] int yBoard = -1;

    // CalcuatePosition()에서 사용하는 임시용 변수
    private int tempX;
    private int tempY;
    // 해당 칸으로 이동할 수 있는지
    private bool canMove = false;

    // 선택되었는지
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; }  set { isSelect = value; } }


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
        MovePosition();
    }
    private void Start()
    {
        // 배열에 값을 넣어준다
        gasValve.SetArray(xBoard, yBoard, index);
    }

    private void Update()
    {
        if (isSelect)
        {
            // 키를 눌러 심볼 움직이기
            MoveSymbol();

        // 좌표 이동
        MovePosition();
        }
    }

    private void MoveSymbol()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 이동할 수 있는지 먼저 테스트
            tempY = yBoard - 1;
            CalculatePosition(xBoard, tempY);

            // 움직일 수 있으면
            if (canMove)
            {
                // array 비우기
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // 인덱스 변경
                yBoard--;
                if (yBoard < 0) yBoard++;

                // array 채우기
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // 이동할 수 있는지 먼저 테스트
            tempY = yBoard + 1;
            CalculatePosition(xBoard, tempY);

            // 움직일 수 있으면
            if (canMove)
            {
                // 현재 array 비우기
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // 인덱스 변경
                yBoard++;
                if (yBoard > 4) yBoard--;

                // array 채우기
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // 이동할 수 있는지 먼저 테스트
            tempX = xBoard - 1;
            CalculatePosition(tempX, yBoard);

            // 움직일 수 있으면
            if (canMove)
            {
                // array 비우기
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // 인덱스 변경
                xBoard--;
                if (xBoard < 0) xBoard++;

                // array 채우기
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // 이동할 수 있는지 먼저 테스트
            tempX = xBoard + 1;
            CalculatePosition(tempX, yBoard);

            // 움직일 수 있으면
            if (canMove)
            {
                // array 비우기
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // 인덱스 변경
                xBoard++;
                if (xBoard > 4) xBoard--;

                // array 채우기
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
    }

    /// <summary>
    /// 가려는 칸이 비어있는지 미리 계산한다
    /// </summary>
    /// <param name="_x"> x좌표 </param>
    /// <param name="_y"> y좌표 </param>
    private void CalculatePosition(int _x, int _y)
    {
        // 범위를 벗어나면 return;
        if (_x < 0 || _y < 0) return;
        if (_x > 4 || _y > 4) return;

        // 가려는 칸이 비었으면
        if (gasValve.IsArrayEmpty(_x, _y))
        {
            // 움직일 수 있음을 표시
            canMove = true;
        }
    }

    private void MovePosition()
    {
        // 좌표 이동
        float x = xBoard * 100;
        float y = yBoard * 100 * -1;

        // 좌표 설정
        rectTransform.anchoredPosition = new Vector3(x, y, 0);

        // 다음을 위해 false;로 
        canMove = false;
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
}