using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform shopInfo;
    [SerializeField] private Transform inventoryInfo;

    public void ShowToolTip(bool show, RectTransform rectTransform, Inventory_Item itemToShow, bool buyPrice = false, bool showShopInfo = false)
    {
        base.ShowToolTip(show, rectTransform);

        shopInfo.gameObject.SetActive(showShopInfo);
        inventoryInfo.gameObject.SetActive(!showShopInfo);

        int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = ($"Price: {price} x {itemToShow.stackSize} - {totalPrice}g");
        string singleStackPrice = ($"Price: {price}g");

        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleStackPrice;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemDescription.text = itemToShow.GetItemDescription();

        string color = GetRarityColor(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color, itemToShow.itemData.itemName);
    }

    private string GetRarityColor(int rarity)
    {
        if (rarity <= 100) return "white"; // common
        if (rarity <= 300) return "green"; // uncommon
        if (rarity <= 600) return "orange"; // rare
        if (rarity <= 800) return "purple"; // epic
        return "yellow"; // legendary
    }
}
