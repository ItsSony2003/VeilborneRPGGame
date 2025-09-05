using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public ItemType slotType;

    private void OnValidate()
    {
        gameObject.name = "UI_EquipmentSlot: " + slotType.ToString();
    }
}
