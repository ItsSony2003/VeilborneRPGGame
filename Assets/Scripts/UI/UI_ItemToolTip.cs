using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(bool show, RectTransform rectTransform, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, rectTransform);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemDescription.text = itemToShow.GetItemDescription();
    }
}
