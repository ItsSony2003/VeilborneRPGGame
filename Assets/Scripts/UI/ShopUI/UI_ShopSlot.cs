using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ShopSlot : UI_ItemSlot
{
    private Inventory_Shop shop;
    public enum ShopSlotType { ShopSlot, PlayerSlot}
    public ShopSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        if (slotType == ShopSlotType.PlayerSlot)
        {
            if (rightButton)
            {
                bool sellFullStack = Input.GetKey(KeyCode.LeftControl);
                shop.TrySellItem(itemInSlot, sellFullStack);
            }
            else if (leftButton)
            {
                return;
            }
        }
        else if (slotType == ShopSlotType.ShopSlot)
        {
            if (rightButton)
            {
                return; // right click does nothing
            }

            // buy item in shop class
            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            shop.TryBuyItem(itemInSlot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }

    public void SetupShopUI(Inventory_Shop shop) => this.shop = shop;
}
