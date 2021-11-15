using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;

    [SerializeField] GameObject G_InventoryBase;
    [SerializeField] GameObject G_ObjectSlotsParent;
    [SerializeField] GameObject G_UseSlotsParent;

    [SerializeField] Slot[] ObjectItemSlots; //������Ʈ ������ ����
    [SerializeField] Slot[] UseItemSlots; // ��� ������ (������ ���� ������) ����

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
        //test Item�߰�
        AcquireItem(i);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            AcquireItem(i2);
        TryOpenInventory();
    }

    public void SlotSelectColorChange(int SlotNum,Color color)
    {
        ObjectItemSlots[SlotNum].SelectColor(color);
    }

    public void TryOpenInventory()
    {
        //PlayerInput�� �����ؾ���
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;
            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    public Item GetSlotItem(int SlotNum)
    {
        return ObjectItemSlots[SlotNum].item;
    }

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
                Debug.Log("������ Ÿ���� ������ ���� ���� �������Դϴ�");
                break;
        }
        
    }

    void AddItemToSlot(Slot[] slots, Item _item, int _Count = 1)
    {
        //�ش� �������� �ִ� ���
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
        //�ش� �������� ���� ���
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _Count);
                return;
            }
        }
    }

    void OpenInventory()
    {
        
        inventoryActivated = true;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        ObjectItemSlots[0].SelectColor(Color.red);
        for (int i = 1; i<ObjectItemSlots.Length; i++)
        {
            ObjectItemSlots[i].SelectColor(Color.white);
        }
    }

    void CloseInventory()
    {
        inventoryActivated = false;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
    }
}
