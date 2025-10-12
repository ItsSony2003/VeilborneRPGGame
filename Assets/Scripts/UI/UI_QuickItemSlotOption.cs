using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;

    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot, Inventory_Item itemToSlot)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateItemSlot(itemToSlot);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }
}
