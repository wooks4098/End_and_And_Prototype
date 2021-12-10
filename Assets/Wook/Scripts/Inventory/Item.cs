using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ObjectItem = 0, // ������Ʈ�� ��ȣ�ۿ��� ���� ������
    UseItem, //��� ������ (���, ����, ��) (������ ���� ������)
}
public enum ItemUseEffect//������ ��� ȿ��
{ 
    None = 0, //����
    HpUp, //ü�� ȸ��
    ThirstyUp, //�񸶸� ȸ��
    InfectionRateDown, //������ �϶�

}

[CreateAssetMenu(fileName = "Item", menuName = "Items/Make New Item", order = 0)]
public class Item : ScriptableObject
{
    public string ItemName; //������ �̸�
    public ItemType itemType; //������ Ÿ��
    public Sprite itemSprite;  //������ ��������Ʈ

    [TextArea]
    public string itemInfo;      //������ ����

    public ItemUseEffect itemUseEffect; //������ ��� ȿ��
    public float UseItemFigure; //��� ������ ��ġ
}