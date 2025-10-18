using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material Data - ")]
public class Item_DataSO : ScriptableObject
{
    public string saveId {  get; private set; }

    [Header("Shop Details")]
    [Range(0, 1200)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("Item Drop Details")]
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100)]
    public float dropChance;
    [Range(0, 100)]
    public float maxDropChance = 70f;

    [Header("Craft Details")]
    public Inventory_Item[] craftRequirements;

    [Header("Item Details")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item Effect Details")]
    public ItemEffect_DataSO itemEffect;

    private void OnValidate()
    {
        dropChance = GetItemDropChance();

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        saveId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public float GetItemDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100; // no item drop chance is 0%, lowest is 0.1%
        
        return Mathf.Min(chance, maxDropChance);
    }
}
