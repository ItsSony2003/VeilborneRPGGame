using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player inventory { get; private set; }
    public List<Inventory_Item> materialStash;

    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumedMaterials(itemToCraft);
        inventory.AddItem(itemToCraft);
    }

    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && inventory.CanAddItem(itemToCraft);
    }

    private void ConsumedMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRequirements)
        {
            int amountToConsume = requiredItem.stackSize;

            amountToConsume -= ConsumedMaterialsAmount(inventory.itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterialsAmount(itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume -= ConsumedMaterialsAmount(materialStash, requiredItem);
        }
    }

    // This function will consume material and return the number it consumed
    // Doing the math and gives you a report of how materials was removed from the inventory
    private int ConsumedMaterialsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;

        foreach (var item in itemList)
        {
            if (item.itemData != neededItem.itemData)
                continue;

            int removedAmount = Mathf.Min(item.stackSize, amountNeeded - consumedAmount);
            item.stackSize -= removedAmount;
            consumedAmount += removedAmount;

            if (item.stackSize <= 0)
                itemList.Remove(item);

            if (consumedAmount >= amountNeeded)
                break;
        }

        return consumedAmount;
    }

    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRequirements)
        {
            if (GetAvailableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }

        return true;
    }

    public int GetAvailableAmountOf(Item_DataSO requiredItem)
    {
        int amount = 0;

        foreach (var item in inventory.itemList)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach (var item in itemList)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach (var item in materialStash)
        {
            if (item.itemData == requiredItem)
                amount += item.stackSize;
        }

        return amount;
    }
    
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableMaterialStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(itemToAdd);
        }

        TriggerUpdateUI();
        materialStash = materialStash.OrderBy(item => item.itemData.name).ToList();
    }

    public Inventory_Item StackableMaterialStash(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = materialStash.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackable in stackableItems)
        {
            if (stackable.CanAddStack())
                return stackable;
        }

        return null;
    }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    public void PlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);

                inventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    public void StorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (inventory.CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);

                RemoveOneItem(item);
                inventory.AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }
}
