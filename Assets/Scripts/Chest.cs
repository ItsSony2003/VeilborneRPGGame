using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Animator anim => GetComponentInChildren<Animator>();

    private Entity_VFX fx => GetComponent<Entity_VFX>();

    public void TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
       anim.SetBool("chestOpen", true);

       // Drop Items
    }
}
