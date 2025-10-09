using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentSlotParent : MonoBehaviour
{
    private UI_EquipmentSlot[] equipmentSlots;

    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equipmentList)
    {
        if (equipmentSlots == null)
            equipmentSlots = GetComponentsInChildren<UI_EquipmentSlot>();

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            var playerEquipmentSlot = equipmentList[i];

            if (playerEquipmentSlot.Hasitem() == false)
                equipmentSlots[i].UpdateItemSlot(null);
            else
                equipmentSlots[i].UpdateItemSlot(playerEquipmentSlot.equipedItem); ;
        }
    }
}
