using System.Collections.Generic;
using UnityEngine;

public class Player_ItemDropManager : Entity_ItemDropManager
{
    [Header("Player Drop Item Details")]
    [Range(0, 100)]
    [SerializeField] private float chanceToLoseItem = 5f;
    private Inventory_Player inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory_Player>();
    }

    public override void DropItems()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.itemList);
        List<Inventory_EquipmentSlot> equipmentCopy = new List<Inventory_EquipmentSlot>(inventory.equipmentList);

        foreach (var item in inventoryCopy)
        {
            if (Random.Range(0, 100) < chanceToLoseItem)
            {
                CreateItemDrop(item.itemData);
                inventory.RemoveFullStack(item);
            }
        }

        foreach (var equip in equipmentCopy)
        {
            if (Random.Range(0, 100) < chanceToLoseItem && equip.Hasitem())
            {
                var item = equip.GetEquipedItem();

                CreateItemDrop(item.itemData);
                inventory.UnequipItem(item);
                inventory.RemoveFullStack(item);
            }
        }
    }
}
