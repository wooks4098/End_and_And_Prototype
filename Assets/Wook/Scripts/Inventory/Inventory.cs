using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;

    [SerializeField] GameObject G_InventoryBase;
    [SerializeField] GameObject G_SlotsParent;

    [SerializeField] Slot[] slots;

    [SerializeField] SlotSelect slotSelect;

    //test
    public Item i;

    private void Start()
    {
        slots = G_SlotsParent.GetComponentsInChildren<Slot>();
        AcquireItem(i);
    }

    private void Update()
    {
        TryOpenInventory();
    }

    public void SlotSelectColorChange(int SlotNum,Color color)
    {
        slots[SlotNum].SelectColor(color);
    }

    public void TryOpenInventory()
    {
        //PlayerInput과 연동해야함
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
        return slots[SlotNum].item;
    }

    public void AcquireItem(Item _item, int _Count = 1)
    {
        //해당 아이템이 있는 경우
        for(int i = 0; i<slots.Length; i++)
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

    void OpenInventory()
    {
        
        inventoryActivated = true;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
        slots[0].SelectColor(Color.red);
        for (int i = 1; i<slots.Length; i++)
        {
            slots[i].SelectColor(Color.white);
        }
    }

    void CloseInventory()
    {
        inventoryActivated = false;
        G_InventoryBase.SetActive(inventoryActivated);
        slotSelect.inventoryActivateChange(inventoryActivated);
    }
}
