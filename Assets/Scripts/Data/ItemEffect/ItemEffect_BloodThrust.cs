using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Blood Thrust", fileName = "Item Effect Data - Heal on dealing Physical Damage")]
public class ItemEffect_BloodThrust : ItemEffect_DataSO
{
    [SerializeField] private float healPercentage = 0.2f;

    public override void Apply(Player player)
    {
        base.Apply(player);

        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Remove()
    {
        base.Remove();

        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * healPercentage);
    }
}
