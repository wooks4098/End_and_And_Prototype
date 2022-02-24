using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchgearPanel : MonoBehaviour
{
    // ������Ʈ
    private Switchgear switchgear;
    private Text panelText;

    // �� ����� ��ȣ - �ܺ�(������)���� ����
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // ���õǾ�����
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; } set { isSelect = value; } }

    // ���� ��
    [SerializeField] int currentValue;
    public int GetCurrentValue() { return currentValue; }


    private void Awake()
    {
        switchgear = GetComponentInParent<Switchgear>();
        panelText = GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        // ���õ��� ���� ����
        isSelect = false;

        // ���� 1�� ����
        currentValue = 1;
    }
    private void Start()
    {
        // �迭 �� ����
        switchgear.SetArray(index, currentValue);
    }
    private void Update()
    {
        // ���õǾ�����
        if (isSelect)
        {
            // Ű�� ���� �г� �� �ٲٱ�
            UpdateValue();

            // text ������Ʈ
            UpdateText();
        }
    }

    private void UpdateValue()
    {        
        if (Input.GetKeyDown(KeyCode.W)) // ��
        {
            // �� ����
            currentValue--;
            if (currentValue < 0) currentValue = 9;

            // �迭 �� ����
            switchgear.SetArray(index, currentValue);
        }
        else if (Input.GetKeyDown(KeyCode.S)) // �Ʒ�
        {
            // �� ����
            currentValue++;
            if (currentValue > 9) currentValue = 0;

            // �迭 �� ����
            switchgear.SetArray(index, currentValue);
        }
    }

    private void UpdateText()
    {
        panelText.text = currentValue.ToString();
    }
}
