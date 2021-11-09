using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 인벤토리의 Slot을 선택하는 클래스
/// Slot이동
/// </summary>

public class SlotSelect : MonoBehaviour
{
    bool inventoryActivated = false;

    Item item; //아이템
    [SerializeField] Image itemImage; //아이템 이미지
    [SerializeField] Text itemName; //아이템 이름
    [SerializeField] Text ItemInfo; //아이템 설명
    int SelectNumber = 0; //0~9;
    Inventory inventory;
    private void Awake()
    {
        inventory = gameObject.GetComponentInParent<Inventory>();
    }
    public void inventoryActivateChange(bool Activate)
    {
        inventoryActivated = Activate;
        if(Activate)
        {
            SelectNumber = 0;
            ChangeItemInfo();

        }
    }

    private void Update()
    {
        if(inventoryActivated)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (SelectNumber >= 5)
                {
                    inventory.SlotSelectColorChange(SelectNumber, Color.white);
                    SelectNumber -= 5;
                    inventory.SlotSelectColorChange(SelectNumber, Color.red);
                    ChangeItemInfo();
                }

            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if (SelectNumber < 5)
                {
                    inventory.SlotSelectColorChange(SelectNumber, Color.white);
                    SelectNumber += 5;
                    inventory.SlotSelectColorChange(SelectNumber, Color.red);
                    ChangeItemInfo();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (SelectNumber != 0 )
                {
                    inventory.SlotSelectColorChange(SelectNumber, Color.white);
                    SelectNumber--;
                    inventory.SlotSelectColorChange(SelectNumber, Color.red);
                    ChangeItemInfo();
                }

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if ( SelectNumber != 9)
                {
                    inventory.SlotSelectColorChange(SelectNumber, Color.white);
                    SelectNumber++;
                    inventory.SlotSelectColorChange(SelectNumber, Color.red);
                    ChangeItemInfo();
                }

            }
        }

       
    }
    public void ChangeItemInfo()
    {
        item = inventory.GetSlotItem(SelectNumber);
        if(item == null)
        {
            itemImage.sprite = null;
            Color color = itemImage.color;
            color.a = 0;
            itemImage.color = color;
            itemName.text = "";
            ItemInfo.text = "";
        }
        else
        {
            itemImage.sprite = item.itemSprite;
            Color color = itemImage.color;
            color.a = 100;
            itemImage.color = color;
            itemName.text = item.ItemName;
            ItemInfo.text = item.itemInfo;
        }
        
    }
}
