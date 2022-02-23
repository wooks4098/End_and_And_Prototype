using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // ������Ʈ
    private RectTransform rectTransform;
    [SerializeField] GasValve gasValve;

    // �� ���(����)�� ��ȣ - �ܺ�(������)���� ����
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // array �ε��� (���� �ε���) - �ܺ�(������)���� ����
    [SerializeField] int xBoard = -1;
    [SerializeField] int yBoard = -1;

    // CalcuatePosition()���� ����ϴ� �ӽÿ� ����
    private int tempX;
    private int tempY;
    // �ش� ĭ���� �̵��� �� �ִ���
    private bool canMove = false;

    // ���õǾ�����
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; }  set { isSelect = value; } }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        // ���õ��� ���� ����
        isSelect = false;

        // �ε����� �°� ��ġ �̵�
        // �迭�� ���߱� ���� y��ǥ�� -1 ����
        MovePosition();
    }
    private void Start()
    {
        // �迭�� ���� �־��ش�
        gasValve.SetArray(xBoard, yBoard, index);
    }

    private void Update()
    {
        if (isSelect)
        {
            // Ű�� ���� �ɺ� �����̱�
            MoveSymbol();

        // ��ǥ �̵�
        MovePosition();
        }
    }

    private void MoveSymbol()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // �̵��� �� �ִ��� ���� �׽�Ʈ
            tempY = yBoard - 1;
            CalculatePosition(xBoard, tempY);

            // ������ �� ������
            if (canMove)
            {
                // array ����
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // �ε��� ����
                yBoard--;
                if (yBoard < 0) yBoard++;

                // array ä���
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // �̵��� �� �ִ��� ���� �׽�Ʈ
            tempY = yBoard + 1;
            CalculatePosition(xBoard, tempY);

            // ������ �� ������
            if (canMove)
            {
                // ���� array ����
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // �ε��� ����
                yBoard++;
                if (yBoard > 4) yBoard--;

                // array ä���
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // �̵��� �� �ִ��� ���� �׽�Ʈ
            tempX = xBoard - 1;
            CalculatePosition(tempX, yBoard);

            // ������ �� ������
            if (canMove)
            {
                // array ����
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // �ε��� ����
                xBoard--;
                if (xBoard < 0) xBoard++;

                // array ä���
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // �̵��� �� �ִ��� ���� �׽�Ʈ
            tempX = xBoard + 1;
            CalculatePosition(tempX, yBoard);

            // ������ �� ������
            if (canMove)
            {
                // array ����
                gasValve.SetArrayEmpty(xBoard, yBoard);

                // �ε��� ����
                xBoard++;
                if (xBoard > 4) xBoard--;

                // array ä���
                gasValve.SetArray(xBoard, yBoard, index);
            }
        }
    }

    /// <summary>
    /// ������ ĭ�� ����ִ��� �̸� ����Ѵ�
    /// </summary>
    /// <param name="_x"> x��ǥ </param>
    /// <param name="_y"> y��ǥ </param>
    private void CalculatePosition(int _x, int _y)
    {
        // ������ ����� return;
        if (_x < 0 || _y < 0) return;
        if (_x > 4 || _y > 4) return;

        // ������ ĭ�� �������
        if (gasValve.IsArrayEmpty(_x, _y))
        {
            // ������ �� ������ ǥ��
            canMove = true;
        }
    }

    private void MovePosition()
    {
        // ��ǥ �̵�
        float x = xBoard * 100;
        float y = yBoard * 100 * -1;

        // ��ǥ ����
        rectTransform.anchoredPosition = new Vector3(x, y, 0);

        // ������ ���� false;�� 
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