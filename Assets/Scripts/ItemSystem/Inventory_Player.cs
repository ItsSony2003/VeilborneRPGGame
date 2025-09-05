using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipmentList;

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
    }

    private void EquipItem(Inventory_Item item, Inventory_EquipmentSlot slot)
    {
        slot.equipedItem = item;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(item);
    }
}
