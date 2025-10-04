using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Shop shop;

    [SerializeField] private UI_ItemSlotParent shopSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;

    public void SetupShopUI(Inventory_Shop shop, Inventory_Player inventory)
    {
        this.shop = shop;
        this.inventory = inventory;

        shop.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_ShopSlot[] shopSlots = GetComponentsInChildren<UI_ShopSlot>();

        foreach (var slot in shopSlots)
            slot.SetupShopUI(shop);
    }

    private void UpdateSlotUI()
    {
        inventorySlots.UpdateSlots(inventory.itemList);
        shopSlots.UpdateSlots(shop.itemList);
    }
}
