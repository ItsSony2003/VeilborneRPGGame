using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();
    private Entity_ItemDropManager itemDropManager => GetComponent<Entity_ItemDropManager>();

    [Header("Open Details")]
    [SerializeField] private bool canDropItems = true;

    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        // Drop Items
        if (canDropItems == false)
            return false;

        canDropItems = false;
        itemDropManager?.DropItems();
        fx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        return true;
    }
}
