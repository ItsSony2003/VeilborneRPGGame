using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_ItemDropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemList_DataSO dropData;

    [Header("Drop Restrictions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemToDrop = 3;

    // TESTING
    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.X))
        //    DropItems();
    }

    public virtual void DropItems()
    {
        if (dropData == null)
        {
            Debug.Log("You need to assign drop data on Entity" + gameObject.name);
            return;
        }

        List<Item_DataSO> itemToDrop = RollItemDrops();
        int amountToDrop = Mathf.Min(itemToDrop.Count, maxItemToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemToDrop[i]);
        }
    }

    protected void CreateItemDrop(Item_DataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_PickupItem>().SetupItem(itemToDrop);
    }

    public List<Item_DataSO> RollItemDrops()
    {
        List<Item_DataSO> possibleDrops = new List<Item_DataSO>();
        List<Item_DataSO> finalDrops = new List<Item_DataSO>();
        float maxRarityAmount = this.maxRarityAmount;

        // STEP 1: Roll each item based on rarity and max drop chance
        foreach (var item in dropData.itemList)
        {
            float dropChance = item.GetItemDropChance();

            if (Random.Range(0, 100) <= dropChance)
                possibleDrops.Add(item);
        }

        // STEP 2: Sort by rarity (highest to lowest)
        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        // STEP 3: Add items to final drop list until rarity limit on entity is reached
        foreach (var item in possibleDrops)
        {
            if (maxRarityAmount > item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount -= item.itemRarity;
            }
        }

        return finalDrops;
    }
}
