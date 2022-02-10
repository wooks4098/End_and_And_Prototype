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

    [SerializeField] Slot[] ObjectItemSlots; //������Ʈ ������ ����
    [SerializeField] Slot[] UseItemSlots; // ��� ������ (������ ���� ������) ����
    [SerializeField] string[] QuickSoltItem;//�����Կ� �� ������


    [SerializeField] SlotSelect slotSelect;
    ItemGet itemget;
    //test
    public Item i;
    public Item i2;
    public Item i3;
    public Item i4;

    private void Start()
    {
        ObjectItemSlots = G_ObjectSlotsParent.GetComponentsInChildren<Slot>();
        UseItemSlots = G_UseSlotsParent.GetComponentsInChildren<Slot>();
        itemget = GetComponent<ItemGet>();
        //input ����
        SetInput();
        //test Item�߰�
        AcquireItem(i2);
        AcquireItem(i3);
        AcquireItem(i4);

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
        //�׽�Ʈ ������ ȹ��Ű
        //if(Input.GetKeyDown(KeyCode.Q))
        //    AcquireItem(i);
        //if (Input.GetKeyDown(KeyCode.T  ))
        //    AcquireItem(i2);
        //if (Input.GetKeyDown(KeyCode.U))
        //    AcquireItem(i3);
        //if (Input.GetKeyDown(KeyCode.O))
        //    AcquireItem(i4);

    }


    //�κ��丮 ������ ���� ���� ����
    public void SlotSelectColorChange(int SlotNum,Color color)
    {
        ObjectItemSlots[SlotNum].SelectColor(color);
    }

    public bool IsHaveItem(string _ItemName)
    {
        //������Ʈ ������ ������ �ִ��� Ȯ��
        for (int i = 0; i < ObjectItemSlots.Length; i++)
        {
            if (ObjectItemSlots[i].item != null)
            {
                if (ObjectItemSlots[i].IsHaveItem(_ItemName) == true)
                    return true;
            }
        }

        //��� ������ ������ �ִ��� Ȯ��
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

    #region ������ ȹ��
    //������ ȹ��
    public void AcquireItem(Item _item, int _Count = 1)
    {
        itemget.ShowItemGetUI(_item);
        switch (_item.itemType)
        {
            case ItemType.ObjectItem:
                AddItemToInventorySlot(ObjectItemSlots, _item, _Count);
                break;
            case ItemType.UseItem:
                AddItemToInventorySlot(UseItemSlots, _item, _Count);
                break;
            default:
                Debug.Log("������ Ÿ���� ������ ���� ���� �������Դϴ�");
                break;
        }
        
    }
    //ȹ���� ������ ���Կ� �߰� (�κ��丮��)
    void AddItemToInventorySlot(Slot[] slots, Item _item, int _Count = 1)
    {
        //�ش� �������� �ִ� ���
        if(!AddItemisNotNall(slots, _item, _Count))
            //�ش� �������� ���� ���
            AdditemNull(slots, _item, _Count);

    }

    //������ �߰� �ش� �������� �ִ°�� | �������� �ִ°�� true��ȯ
    bool AddItemisNotNall(Slot[] slots, Item _item, int _Count = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.ItemName == _item.ItemName)
                {
                    slots[i].SetSoltCount(_Count);
                    return true;
                }
            }
        }
        return false;
    }

    //������ �߰� �ش� �������� ���°��
    void AdditemNull(Slot[] slots, Item _item, int _Count = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _Count);
                return;
            }
        }
    }

    //ȹ���� ������ ���Կ� �߰� (�����Կ�)
    void AddItemToQuickSlot(Slot[] slots, Item _item, int _Count = 1)
    {
        switch (_item.ItemName)
        {
            case "����":
                AddQuickItem(slots[0], _item, _Count);
                break;
            case "��":
                AddQuickItem(slots[1], _item, _Count);
                break;
            case "���":
                AddQuickItem(slots[2], _item, _Count);
                break;
        }
    }

    void AddQuickItem(Slot slot, Item _item, int _Count = 1)
    {
        if(slot.item != null)
        {
            if (slot.item.ItemName == _item.ItemName)
            {
                slot.SetSoltCount(_Count);
                return;
            }
        }
        if (slot.item == null)
        {
            slot.AddItem(_item, _Count);
            return;
        }
    }
    #endregion

    #region �κ��丮 Open Close

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
        //�κ��丮 ������Ʈ �Ӽ� ����
        inventoryActivated = true;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        //�κ��丮 ���� ���� �ʱ�ȭ
        ObjectItemSlots[0].SelectColor(Color.red);
        for (int i = 1; i<ObjectItemSlots.Length; i++)
        {
            ObjectItemSlots[i].SelectColor(Color.white);
        }
        //�÷��̾� ���º���
        GameManager.Instance.PlayerStateChange(playerType, PlayerState.Inventory);
    }

    void CloseInventory()
    {
        //�κ��丮 ������Ʈ �Ӽ� ����
        inventoryActivated = false;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        //�÷��̾� ���º���
        GameManager.Instance.PlayerStateChange(playerType, PlayerState.Walk);
    }
    #endregion


    public Item GetSlotItem(int SlotNum)
    {
        return ObjectItemSlots[SlotNum].item;
    }
}
