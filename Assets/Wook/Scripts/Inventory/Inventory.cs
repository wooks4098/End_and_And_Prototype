using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;

    [SerializeField] PlayerType playerType;
    [SerializeField] GameObject G_InventoryBase;
    [SerializeField] GameObject G_ObjectSlotsParent;
    [SerializeField] GameObject G_UseSlotsParent;

    [SerializeField] Slot[] ObjectItemSlots; //오브젝트 아이템 슬롯
    [SerializeField] Slot[] UseItemSlots; // 사용 아이템 (퀵슬롯 전용 아이템) 슬롯

    [SerializeField] SlotSelect slotSelect;
    ItemGet itemget;
    //test
    public Item i;
    public Item i2;

    private void Start()
    {
        ObjectItemSlots = G_ObjectSlotsParent.GetComponentsInChildren<Slot>();
        UseItemSlots = G_UseSlotsParent.GetComponentsInChildren<Slot>();
        itemget = GetComponent<ItemGet>();
        //input 연결
        SetInput();
        //test Item추가
        AcquireItem(i2);

    }

    void SetInput()
    {
        switch(playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnInventoryOpenPlayer1 += TryOpenInventory;
                break;

            case PlayerType.SecondPlayer:
                InputManager.Instance.OnInventoryOpenPlayer2 += TryOpenInventory;
                break;
        }
    }


    private void Update()
    {
        //테스트 아이템 획득키
        if(Input.GetKeyDown(KeyCode.Q))
            AcquireItem(i);

    }

    //인벤토리 아이템 선택 색상 변경
    public void SlotSelectColorChange(int SlotNum,Color color)
    {
        ObjectItemSlots[SlotNum].SelectColor(color);
    }

    public bool IsHaveItem(string _ItemName)
    {
        //오브젝트 아이템 가지고 있는지 확인
        for (int i = 0; i < ObjectItemSlots.Length; i++)
        {
            if (ObjectItemSlots[i].item != null)
            {
                if (ObjectItemSlots[i].IsHaveItem(_ItemName) == true)
                    return true;
            }
        }

        //사용 아이템 가지고 있는지 확인
        for (int i = 0; i < UseItemSlots.Length; i++)
        {
            if (UseItemSlots[i].item != null)
            {
                if (UseItemSlots[i].IsHaveItem(_ItemName) == true)
                    return true;
            }
        }
        return false;
    }

    #region 아이템 획득
    //아이템 획득
    public void AcquireItem(Item _item, int _Count = 1)
    {
        itemget.ShowItemGetUI(_item);
        switch (_item.itemType)
        {
            case ItemType.ObjectItem:
                AddItemToSlot(ObjectItemSlots, _item, _Count);
                break;
            case ItemType.UseItem:
                AddItemToSlot(UseItemSlots, _item, _Count);
                break;
            default:
                Debug.Log("아이템 타입이 정해져 있지 않은 아이템입니다");
                break;
        }
        
    }
    //획득한 아이템 슬롯에 추가
    void AddItemToSlot(Slot[] slots, Item _item, int _Count = 1)
    {
        //해당 아이템이 있는 경우
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.ItemName == _item.ItemName)
                {
                    slots[i].SetSoltCount(_Count);
                    return;
                }
            }
        }
        //해당 아이템이 없는 경우
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _Count);
                return;
            }
        }
    }
    #endregion

    #region 인벤토리 Open Close

    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;
        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }
    void OpenInventory()
    {
        //인벤토리 오브젝트 속성 변경
        inventoryActivated = true;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        //인벤토리 선택 색상 초기화
        ObjectItemSlots[0].SelectColor(Color.red);
        for (int i = 1; i<ObjectItemSlots.Length; i++)
        {
            ObjectItemSlots[i].SelectColor(Color.white);
        }
        //플레이어 상태변경
        GameManager.Instance.PlayerStateChange(playerType, PlayerState.Inventory);
    }

    void CloseInventory()
    {
        //인벤토리 오브젝트 속성 변경
        inventoryActivated = false;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        //플레이어 상태변경
        GameManager.Instance.PlayerStateChange(playerType, PlayerState.Walk);
    }
    #endregion


    public Item GetSlotItem(int SlotNum)
    {
        return ObjectItemSlots[SlotNum].item;
    }
}
