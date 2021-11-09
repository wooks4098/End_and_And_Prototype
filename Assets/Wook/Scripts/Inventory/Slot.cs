using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public Item item; //아이템
    public int itemCount; //아이탬 개수
    public Image itemImage; //아이템 이미지
    public Image SelectImage; //아이템 틀 이미지
    [SerializeField]
    Text text_Count;
    [SerializeField]
    private GameObject G_CountImage;

    //이미지 투명도 조절
    void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //슬롯 선택 이미지 조절
    public void SelectColor(Color _color)
    {
        SelectImage.color = _color;
    }

    //아이템 획득
    public void AddItem(Item _item, int _Count = 1)
    {
        item = _item;
        itemCount = _Count;
        itemImage.sprite = item.itemSprite;

        //개수 표시
        G_CountImage.SetActive(true);
        text_Count.text = itemCount.ToString();
        SetColor(1);
    }
    //아이템 개수 조정
    public void SetSoltCount(int _Count)
    {
        itemCount += _Count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    //슬롯 초기화
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
