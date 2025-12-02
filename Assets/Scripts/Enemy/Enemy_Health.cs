using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (canTakeDamage == false)
            return false;

        bool wasHit = base.TakeDamage(damage, elementalDamage, element, damageDealer);

        if (wasHit == false)
            return false;

        // try enter battle state
        // if(damageDealer.GetComponent<Player>() != null)
        if (damageDealer.CompareTag("Player"))
            enemy.TryEnterBattleState(damageDealer);

        return true;
    }

    protected override void Die()
    {
        base.Die();

        // only bosses trigger victory UI
        if (enemy.isBoss)
        {
            UI.instance.OpenVictoryUI();
        }

        Destroy(gameObject, 3);
    }
}
