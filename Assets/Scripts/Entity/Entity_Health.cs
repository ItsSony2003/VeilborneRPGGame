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

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        
        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHp(physicalDamageTaken + elementalDamageTaken);

        //Debug.Log("Elemental damage Taken: " + elementalDamageTaken + " element: " + element);

        return true;
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculationKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);
    }

    private bool AttackEvaded() => Random.Range(0, 100) <= stats.GetEvasion();

    protected void ReduceHp(float damage)
    {
        entityVfx?.PlayOnDamageVfx();
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
