using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // ������Ʈ
    private RectTransform rectTransform;
    public GameObject goController;
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
    private void Start()
    {
        // ���õ��� ���� ����
        isSelect = false;

        // �ε����� �°� ��ġ �̵�
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
        // ���� ��Ʈ�ѷ� ã��
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