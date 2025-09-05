using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] uiItemSlots;
    private UI_EquipmentSlot[] uiEquipmentSlots;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipmentSlotParent;

    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipmentSlots = uiEquipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipmentList = inventory.equipmentList; // the correct, sized list

        for (int i = 0; i < uiEquipmentSlots.Length; i++)
        {
            var playerEquipmentSlot = playerEquipmentList[i];

            if (playerEquipmentSlot.Hasitem() == false)
                uiEquipmentSlots[i].UpdateItemSlot(null);
            else
                uiEquipmentSlots[i].UpdateItemSlot(playerEquipmentSlot.equipedItem);
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < uiItemSlots.Length; i++)
        {
            if (i <  itemList.Count)
            {
                uiItemSlots[i].UpdateItemSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateItemSlot(null);
            }
        }
    }
}
