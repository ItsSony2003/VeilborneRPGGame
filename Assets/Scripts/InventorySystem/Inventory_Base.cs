using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChange;

    public int maxInventorySize = 12;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

    }

    public void TryUseITem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);

        if (consumable == null)
            return;

        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = StackableItems(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }

    public Inventory_Item StackableItems(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stack in stackableItems)
        {
            if (stack.CanAddStack())
                return stack;
        }

        return null;
    }

    public virtual void AddItem(Inventory_Item itemToAdd)
    {
        //if (itemToAdd.IsMaterial())
        //{
        //    AddMaterial(itemToAdd.itemData);
        //    return;
        //}

        Inventory_Item existingStackable = StackableItems(itemToAdd);
        if (existingStackable != null)
            existingStackable.AddStack();
        else
            itemList.Add(itemToAdd);

        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);
        
        if (itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemToRemove);

        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(Item_DataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
    
    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
