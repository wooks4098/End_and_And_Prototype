using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;

    [SerializeField] GameObject G_InventoryBase;
    [SerializeField] GameObject G_SlotsParent;

    Slot[] slots;

    private void Start()
    {
        slots = G_SlotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        TryOpenInventory();
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

    public void AcquireItem(Item _item, int _Count)
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
        G_InventoryBase.SetActive(true);

    }

    void CloseInventory()
    {
        G_InventoryBase.SetActive(false);

    }
}
