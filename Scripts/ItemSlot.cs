using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemSlot
{
    public ItemData slotItem;
    public int slotIndex;
    public int itemCount;
    public Inventory inven;
    public bool IsSelected { get; set; } = false;

    public UnityAction onSelectChange;

    public ItemSlot(Inventory slotInven)
    {
        inven = slotInven;
    }

    public void SetSlot(int index)
    {
        slotIndex = index;
    }

    public void SetItem(ItemData item)
    {
        slotItem = item;
        itemCount = 1;
        inven.onInventoryChanged?.Invoke();
    }

    public void AddItemCount()
    {
        itemCount++;
        inven.onInventoryChanged?.Invoke();
    }

    public void ClearSlot()
    {
        slotItem = null;
        itemCount = 0;
        SelectSlot(false);
    }

    public void SelectSlot(bool select)
    {
        IsSelected = select;
        onSelectChange?.Invoke();
    }
}
