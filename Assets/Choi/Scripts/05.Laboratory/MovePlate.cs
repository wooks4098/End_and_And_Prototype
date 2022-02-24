using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    // ������Ʈ
    private GasValve gasValve;
    private RectTransform rectTransform;
    private Image image;

    // ���(����) �ε���
    private int symbolIndex;
    // ���� ���(����)
    [SerializeField] RectTransform goCurrentSymbol;
    // ���� ��ǥ�� ������ ����
    private float currentXPos;
    private float currentYPos;

    // false: ��� ���� ����, true: ��� ��ġ �̵� ����
    public bool isConfirm = false;


    private void Awake()
    {
        gasValve = GetComponentInParent<GasValve>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void Start()
    {
        // �ε��� �ʱ�ȭ
        symbolIndex = 0;

        // ��ǥ(��ġ) ����
        SetPosition();
    }

    /// <summary>
    /// ��ǥ ���� - ������ �߽�����
    /// </summary>
    private void SetPosition()
    {
        // ���� ���(����) �ʱ�ȭ
        goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

        // ��ǥ �ʱ�ȭ
        currentXPos = goCurrentSymbol.anchoredPosition.x;
        currentYPos = goCurrentSymbol.anchoredPosition.y;

        // ��ǥ ����
        rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
    }

    private void Update()
    {
        // ����(Ȯ��) Ű �Է�
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
            // ���� ����
            ChangeColor();

            // ���� �ɺ��� ���������Ǿ����� ǥ��
            goCurrentSymbol.GetComponent<GasValveSymbol>().IsSelect = false;

            SelectSymbolIndex();
        }
        else if(isConfirm)
        {
            // ���� ����
            ChangeColor();

            // ���� �ɺ��� ���õǾ����� ǥ��
            goCurrentSymbol.GetComponent<GasValveSymbol>().IsSelect = true;

            // �ɺ��� ���� ��ǥ ������Ʈ
            // �ɺ��� isSelect = true;�̸� Ű�� ���� ������ �� �ִ�.
            SetPosition();
        }
    }

    private void SelectSymbolIndex()
    {
        // ����Ű
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            // �ε��� ����
            symbolIndex++;
            if (symbolIndex > 4) symbolIndex = 0;

            // ��ǥ(��ġ) ����
            SetPosition();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            // �ε��� ����
            symbolIndex--;
            if (symbolIndex < 0) symbolIndex = 4;

            // ��ǥ(��ġ) ����
            SetPosition();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void ChangeColor()
    {
        // ���� ����
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
