using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public int gold = 100000;

    private Player player;
    public List<Inventory_EquipmentSlot> equipmentList; // DO NOT TOUCH THIS OR IT WILL CRASH FOR SOME REASON
    public Inventory_Storage storage {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
        storage = FindAnyObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
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

        UnequipItem(itemToEquip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToAdd, Inventory_EquipmentSlot slot)
    {
        float healthPercent = player.health.GetHealthPercent();

        slot.equipedItem = itemToAdd;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddItemEffect(player);

        player.health.ConvertHealthToPercent(healthPercent);
        RemoveOneItem(itemToAdd);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
        {
            Debug.Log("No Space Left!!");
            return;
        }

        float healthPercent = player.health.GetHealthPercent();
        var slotToUnequip = equipmentList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();

        player.health.ConvertHealthToPercent(healthPercent);
        AddItem(itemToUnequip);
    }
}
