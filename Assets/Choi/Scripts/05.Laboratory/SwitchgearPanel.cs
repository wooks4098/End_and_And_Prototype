using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchgearPanel : MonoBehaviour
{
    // ������Ʈ
    private UnityEngine.UI.Outline outline;

    // �� ����� ��ȣ - �ܺ�(������)���� ����
    [SerializeField] int index;
    public int GetIndex() { return index; }

    // ���õǾ�����
    [SerializeField] bool isSelect = false;
    public bool IsSelect { get { return isSelect; } set { isSelect = value; } }


    private void Awake()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();
    }
    private void OnEnable()
    {
        // ���õ��� ���� ����
        isSelect = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }

        // ���õǾ�����
        if (isSelect)
        {
            // Ű�� ���� �ɺ� �����̱�
            ChangeValue();
        }
    }

    private void ChangeValue()
    {
        
    }
}
