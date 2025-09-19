using UnityEngine;

public class Object_ShopMerchant : Object_NPC, IInteractable
{
    public void Interact()
    {
        Debug.Log("Open Shop!!!");
    }
}
