using System.Collections.Generic;
using UnityEngine;

public class Inventory_Shop : Inventory_Base
{
    private Inventory_Player inventory;

    [SerializeField] private ItemList_DataSO shopData;
    [SerializeField] private int minItemAmount = 3;

    protected override void Awake()
    {
        base.Awake();

        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for (int i = 0; i < amountToBuy; i++)
        {
            if (inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("You're poor!");
                return;
            }

            if (itemToBuy.itemData.itemType == ItemType.Material)
            {
                inventory.storage.AddMaterialToStash(itemToBuy);
            }
            else
            {
                if (inventory.CanAddItem(itemToBuy))
                {
                    var itemToAdd = new Inventory_Item(itemToBuy.itemData);
                    inventory.AddItem(itemToAdd);
                }
            }

            inventory.gold -= itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);
        }

        TriggerUpdateUI();
    }

    public void TrySellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for (int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            inventory.gold += sellPrice;
            inventory.RemoveOneItem(itemToSell);
        }

        TriggerUpdateUI();
    }

    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> shopItems = new List<Inventory_Item>();

        foreach (var itemData in shopData.itemList)
        {
            int randomStackAmount = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStackAmount = Mathf.Clamp(randomStackAmount, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStackAmount;

            shopItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemAmount, maxInventorySize + 1);
        int finalItemAmount = Mathf.Clamp(randomItemAmount, 1, shopItems.Count);

        for (int i = 0; i < finalItemAmount; i++)
        {
            var randomIndex = Random.Range(0, shopItems.Count);
            var item = shopItems[randomIndex];

            if (CanAddItem(item))
            {
                shopItems.Remove(item);
                AddItem(item);
            }
        }

        TriggerUpdateUI();
    }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;
}
