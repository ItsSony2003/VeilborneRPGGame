using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemId;

    public Item_DataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {  get; private set; }
    public ItemEffect_DataSO itemEffect;

    public Inventory_Item(Item_DataSO itemData)
    {
        this.itemData = itemData;
        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffect;

        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.AddModifier(modifier.value, itemId);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    private Equipment_DataSO EquipmentData()
    {
        if (itemData is  Equipment_DataSO equipment)
            return equipment;

        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
