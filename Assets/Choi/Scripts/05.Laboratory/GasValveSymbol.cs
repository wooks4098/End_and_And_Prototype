using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // ������Ʈ
    private RectTransform rectTransform;
    public GameObject goMovePlate;

    // array �ε��� (���� �ε���) - �ܺ�(������)���� ����
    [SerializeField] int xBoard = -1;
    [SerializeField] int yBoard = -1;

    // ���õǾ�����
    public bool isSelect = false;

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
        float x = xBoard * 100;
        float y = yBoard * 100 * -1;

        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }
    private void Start()
    {
    }

        
    /// <param name="_num"> ��� �� ���� ��ȯ�� ���� ����ϴ� �Ķ���� </param>
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