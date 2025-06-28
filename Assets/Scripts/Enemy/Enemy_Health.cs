using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (isDead)
            return;

        // try enter battle state
        // if(damageDealer.GetComponent<Player>() != null)
        if (damageDealer.CompareTag("Player"))
            enemy.TryEnterBattleState(damageDealer);
    }
}
