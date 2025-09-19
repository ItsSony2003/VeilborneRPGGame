using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_EquipmentSlot[] uiEquipmentSlots;

    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private Transform uiEquipmentSlotParent;

    private void Awake()
    {
        uiEquipmentSlots = uiEquipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
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
}
