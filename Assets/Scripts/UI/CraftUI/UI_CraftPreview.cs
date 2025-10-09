using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewSlot[] craftRequirementSlots;

    [Header("Item Preview Setup Details")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        craftRequirementSlots = GetComponentsInChildren<UI_CraftPreviewSlot>();
        foreach (var slot in craftRequirementSlots)
            slot.gameObject.SetActive(false);
    }

    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            buttonText.text = "Pick an item";

            return;
        }

        if (storage.CanCraftItem(itemToCraft))
            storage.CraftItem(itemToCraft);

        UpdateCraftPreviewSlots();
    }

    public void UpdateCraftPreview(Item_DataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemDescription();
        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftRequirementSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRequirements.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRequirements[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftRequirementSlots[i].gameObject.SetActive(true);
            craftRequirementSlots[i].SetupMaterialSlot(requiredItem.itemData, availableAmount, requiredAmount);
        }
    }
}
