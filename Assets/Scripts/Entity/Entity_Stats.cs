using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceStats resources;
    public Stat_OffenseStats offense;
    public Stat_DefenseStats defense;
    public Stat_MajorStats major;

    public float GetElementalDamage(out ElementType element)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        //float darknessDamage = offense.darknessDamage.GetValue();
        //float poisonDamage = offense.poisonDamage.GetValue();

        float bonusElementalDamage = major.intelligence.GetValue(); // bonus elemental damage from intelligence +1 per intelligence

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        //if (darknessDamage > highestDamage)
        //{
        //    highestDamage = darknessDamage;
        //    element = ElementType.Darkness;
        //}

        //if (poisonDamage > highestDamage)
        //{
        //    highestDamage = poisonDamage;
        //    element = ElementType.Poison;
        //}

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = (element == ElementType.Fire) ? 0 : fireDamage * 0.5f;
        float bonusIce = (element == ElementType.Ice) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (element == ElementType.Lightning) ? 0 : lightningDamage * 0.5f;
        //float bonusPoison = (element == ElementType.Poison) ? 0 : poisonDamage * 0.5f;
        //float bonusDarkness = (element == ElementType.Darkness) ? 0 : darknessDamage * 0.5f;

        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + weakerElementDamage + bonusElementalDamage;

        return finalDamage;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * 0.5f; // Bonus resistance from intelligence: +0.5% per Intelligence

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 70f; // maximum resistance is 70%
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistance;
    }

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

    public float GetArmorMitigation(float armorReduction) // reduce damage taken
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); // Bonus armor from vitality: +1 per Vitality
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.7f; // max mitigation will be capped at 70%

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction() // penetrate armor
    {
        // total armor reduction as multiplier (e.g: 30 / 100 = 0.3 * multiplier)
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.4f; //each agility point gives you 0.4% chance of evasion

        float totalEvation = baseEvasion + bonusEvasion;
        float evasionCap = 25f; // maximum evasion chance

        float finalEvasion = Mathf.Clamp(totalEvation, 0, evasionCap);

        return finalEvasion;
    }

    public float GetMaxHealth()
    {
        float baseHealth = resources.maxHealth.GetValue();
        float bonusHealth = major.vitality.GetValue() * 5;

        float finalMaxHealth = baseHealth + bonusHealth;

        return finalMaxHealth;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.IceDamage: return offense.iceDamage;
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.LightningDamage: return offense.lightningDamage;
            //case StatType.PoisonDamage: return offense.poisonDamage;
            //case StatType.DarknessDamage: return offense.darknessDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.IceResistance: return defense.iceRes;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.LightningResistance: return defense.lightningRes;
            //case StatType.PoisonResistance: return defense.poisonRes;
            //case StatType.DarknessDamage: return defense.darknessRes;

            default:
                Debug.LogWarning($"StatType {type} not implement yet.");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No Stat Setup yet");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.intelligence);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);
        //offense.poisonDamage.SetBaseValue(defaultStatSetup.poisonDamage);
        //offense.darknessDamage.SetBaseValue(defaultStatSetup.darknessDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
        //defense.poisonRes.SetBaseValue(defaultStatSetup.poisonResistance);
        //defense.darknessRes.SetBaseValue(defaultStatSetup.darknessResistance);
    }
}
