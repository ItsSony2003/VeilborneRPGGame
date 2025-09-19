using UnityEngine;

public class Object_PickupItem : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Item_DataSO itemData;

    private void OnValidate()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_PickupItem - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Player playerInventory = collision.GetComponent<Inventory_Player>();
        Inventory_Storage storage = playerInventory.storage;

        if (itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        if (playerInventory.CanAddItem(itemToAdd))
        {
            playerInventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
