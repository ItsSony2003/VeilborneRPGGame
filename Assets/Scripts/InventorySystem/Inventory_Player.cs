using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;
    [SerializeField] private ItemList_DataSO itemDataBase;

    public List<Inventory_EquipmentSlot> equipmentList; // DO NOT TOUCH THIS OR IT WILL CRASH FOR SOME REASON
    public Inventory_Storage storage {  get; private set; }

    [Header("Quick Item Slot")]
    public Inventory_Item[] quickItems = new Inventory_Item[2];

    [Header("Gold Info")]
    public int gold = 100000;

    protected override void Awake()
    {
        base.Awake();

        storage = FindAnyObjectByType<Inventory_Storage>();
    }

    public void SetupQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        TriggerUpdateUI();
    }

    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        if (itemToUse == null)
            return;

        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotNumber] = FindSameItem(itemToUse);
        }

        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);
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

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.inventory.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveId;

                if (data.inventory.ContainsKey(saveId) == false)
                    data.inventory[saveId] = 0;

                data.inventory[saveId] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;

        foreach (var item in data.inventory)
        {
            string saveId = item.Key;
            int stackSize = item.Value;

            Item_DataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not Found: " + saveId);
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        TriggerUpdateUI();
    }
}
