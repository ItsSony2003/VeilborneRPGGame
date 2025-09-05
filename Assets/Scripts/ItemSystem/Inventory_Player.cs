using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipmentList; // DO NOT TOUCH THIS OR IT WILL CRASH FOR SOME REASON

    protected override void Awake()
    {
        base.Awake();

        playerStats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var equipmentSlots = equipmentList.FindAll(slot => slot.slotType == item.itemData.itemType);

        // STEP 1: Try to find empty slot and equip item
        foreach ( var slot in equipmentSlots)
        {
            if (slot.Hasitem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        // STEP 2: No empty slots? Replace first one
        var slotToReplace = equipmentSlots[0];
        var itemToEquip = slotToReplace.equipedItem;

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToEquip);
    }

    private void EquipItem(Inventory_Item itemToAdd, Inventory_EquipmentSlot slot)
    {
        slot.equipedItem = itemToAdd;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(itemToAdd);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log("No Space Left!!");
            return;
        }

        foreach ( var slot in equipmentList)
        {
            if (slot.equipedItem == itemToUnequip)
            {
                slot.equipedItem = null;
                break;
            }
        }

        itemToUnequip.RemoveModifiers(playerStats);
        AddItem(itemToUnequip);
    }
}
