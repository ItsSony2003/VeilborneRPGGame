using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;
    public Stat_MajorStats major;
    public Stat_OffenseStats offense;
    public Stat_DefenseStats defense;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f; // Bonus Crit chance from agility, 0.3% for each point
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f; // Bonus Crit chance from strength, 0.5% for each point
        float critPower = (baseCritPower + bonusCritPower) / 100; // Total crit power as multiplier (e.g: 100/100 = 1f multiplier)

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? (totalBaseDamage * critPower) : totalBaseDamage;

        return finalDamage;
    }

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        
        float finalMaxHp = baseHp + bonusHp;

        return finalMaxHp;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.4f; //each agility point gives you 0.4% chance of evasion

        float totalEvation = baseEvasion + bonusEvasion;
        float maxEvasion = 25f; // maximum evasion chance

        float finalEvasion = Mathf.Clamp(totalEvation, 0, maxEvasion);

        return finalEvasion;
    }
}
