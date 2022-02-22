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

    GameObject reference = null;

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

        // ���� ���(����) �ʱ�ȭ
        goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

        // ��ǥ �ʱ�ȭ
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
            // ���� ���(����) �ʱ�ȭ
            goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

            // ��ǥ �ʱ�ȭ
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
            // ���� ���(����) �ʱ�ȭ
            goCurrentSymbol = gasValve.GetSymbol(symbolIndex).GetComponent<RectTransform>();

            // ��ǥ �ʱ�ȭ
            currentXPos = goCurrentSymbol.anchoredPosition.x;
            currentYPos = goCurrentSymbol.anchoredPosition.y;

            rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
        }
    }
    
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
       

    public void SetPosition(int _xIndex, int _yIndex)
    {
        // ��� ��ġ�� ���� �̵�
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
