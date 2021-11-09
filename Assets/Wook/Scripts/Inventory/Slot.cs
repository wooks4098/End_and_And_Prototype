using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public Item item; //������
    public int itemCount; //������ ����
    public Image itemImage; //������ �̹���
    public Image SelectImage; //������ Ʋ �̹���
    [SerializeField]
    Text text_Count;
    [SerializeField]
    private GameObject G_CountImage;

    //�̹��� ���� ����
    void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //���� ���� �̹��� ����
    public void SelectColor(Color _color)
    {
        SelectImage.color = _color;
    }

    //������ ȹ��
    public void AddItem(Item _item, int _Count = 1)
    {
        item = _item;
        itemCount = _Count;
        itemImage.sprite = item.itemSprite;

        //���� ǥ��
        G_CountImage.SetActive(true);
        text_Count.text = itemCount.ToString();
        SetColor(1);
    }
    //������ ���� ����
    public void SetSoltCount(int _Count)
    {
        itemCount += _Count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    //���� �ʱ�ȭ
    void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        G_CountImage.SetActive(false);

    }
}
