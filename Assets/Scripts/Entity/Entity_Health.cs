using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;
    private Entity entity;
    private Entity_Stats stats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = 0.15f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.8f, 2.5f);

    [Header("On Heavy Damage Knockback")]
    [Range(0f, 1f)]
    [SerializeField] private float heavyDamageThreshold = 0.2f; // percent of maxHp should lose to get heavy knockback
    [SerializeField] private float heavyKnockbackDuration = 0.45f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(12f, 5f);

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) 
            return;

        float duration = CalculateDuration(damage);
        Vector2 knockback = CalculationKnockback(damage, damageDealer);

        entityVfx?.PlayOnDamageVfx();
        entity?.ReceiveKnockback(knockback, duration);
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        // Entity Died
        entity.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    private Vector2 CalculationKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        
        knockback.x *= direction;

        return knockback;

    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() > heavyDamageThreshold;
}
