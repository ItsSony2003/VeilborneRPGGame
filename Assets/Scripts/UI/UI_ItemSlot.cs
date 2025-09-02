using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    public Inventory_Item itemInSlot {  get; private set; }

    [Header("UI Stat Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;
    
    public void UpdateItemSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = 0.85f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        //itemStackSize.text = item.stackSize.ToString();
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }
}
