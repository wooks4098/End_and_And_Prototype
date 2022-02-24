using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlate : MonoBehaviour
{
    // ������Ʈ
    private Switchgear switchgear;
    private RectTransform rectTransform;
    private Image image;

    // �г� �ε���
    private int panelIndex;
    // ���� ���(����)
    [SerializeField] RectTransform goCurrentPanel;
    // ���� ��ǥ�� ������ ����
    private float currentXPos;
    private float currentYPos;

    // false: �г� ���� ����, true: �г� �� ���� ����
    public bool isConfirm = false;


    private void Awake()
    {
        switchgear = GetComponentInParent<Switchgear>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void Start()
    {
        // �ε��� �ʱ�ȭ
        panelIndex = 0;

        // ��ǥ(��ġ) ����
        SetPosition();
    }

    /// <summary>
    /// ��ǥ ���� - ������ �߽�����
    /// </summary>
    private void SetPosition()
    {
        // ���� �г� �ʱ�ȭ
        goCurrentPanel = switchgear.GetPanel(panelIndex).GetComponent<RectTransform>();

        // ��ǥ �ʱ�ȭ
        currentXPos = goCurrentPanel.anchoredPosition.x;
        currentYPos = goCurrentPanel.anchoredPosition.y;

        // ��ǥ ����
        rectTransform.anchoredPosition = new Vector3(currentXPos, currentYPos, 0);
    }

    private void Update()
    {
        // ����(Ȯ��) Ű �Է�
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
            // ���� ����
            ChangeColor();

            // ���� �ɺ��� ���������Ǿ����� ǥ��
            goCurrentPanel.GetComponent<SwitchgearPanel>().IsSelect = false;

            SelectPanelIndex();
        }
        else if (isConfirm)
        {
            // ���� ����
            ChangeColor();

            // ���� �ɺ��� ���õǾ����� ǥ��
            goCurrentPanel.GetComponent<SwitchgearPanel>().IsSelect = true;

            // �ɺ��� ���� ��ǥ ������Ʈ
            // �ɺ��� isSelect = true;�̸� Ű�� ���� ������ �� �ִ�.
            SetPosition();
        }
    }

    private void SelectPanelIndex()
    {
        // ����Ű
        if (Input.GetKeyDown(KeyCode.D))
        {
            // �ε��� ����
            panelIndex++;
            if (panelIndex > 4) panelIndex = 0;

            // ��ǥ(��ġ) ����
            SetPosition();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // �ε��� ����
            panelIndex--;
            if (panelIndex < 0) panelIndex = 4;

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
