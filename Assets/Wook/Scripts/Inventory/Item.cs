using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ObjectItem = 0, // 오브젝트와 상호작용을 위한 아이템
    UseItem, //사용 아이템 (백신, 음식, 물)
}

[CreateAssetMenu(fileName = "Item", menuName = "Items/Make New Item", order = 0)]
public class Item : ScriptableObject
{
    public string ItemName; //아이템 이름
    public ItemType itemType; //아이템 타입
    public Sprite itemSprite;  //아이템 스프라이트

    [TextArea]
    public string itemInfo;      //아이템 설명

}