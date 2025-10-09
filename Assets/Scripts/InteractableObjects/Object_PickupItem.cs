using UnityEngine;

public class Object_PickupItem : MonoBehaviour
{
    [SerializeField] private Vector2 dropSpeed = new Vector2(4, 12);
    [SerializeField] private Item_DataSO itemData;

    [Space]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    private void OnValidate()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    public void SetupItem(Item_DataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xDropSpeed = Random.Range(-dropSpeed.x, dropSpeed.x);
        rb.linearVelocity = new Vector2 (xDropSpeed, dropSpeed.y);
        col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_PickupItem - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player playerInventory = collision.GetComponent<Inventory_Player>();

        if (playerInventory == null)
            return;
        
        Inventory_Item itemToAdd = new Inventory_Item(itemData);
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
