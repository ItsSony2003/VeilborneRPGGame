using UnityEngine;

public class Object_ShopMerchant : Object_NPC, IInteractable
{
    private Inventory_Player inventory;
    private Inventory_Shop shop;

    protected override void Awake()
    {
        base.Awake();

        shop = GetComponent<Inventory_Shop>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            shop.FillShopList();
        }
    }

    public void Interact()
    {
        ui.shopUI.SetupShopUI(shop, inventory);
        ui.OpenShopUI(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        inventory = player.GetComponent<Inventory_Player>();
        shop.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        ui.HideAllToolTips();
        ui.OpenShopUI(false);
    }
}
