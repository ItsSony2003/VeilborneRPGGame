using System.Collections.Generic;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;
    public Stat_MajorStats major;
    public Stat_OffenseStats offense;
    public Stat_DefenseStats defense;

    public float GetElementalDamage(out ElementType element)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        //float darknessDamage = offense.darknessDamage.GetValue();
        //float poisonDamage = offense.poisonDamage.GetValue();

        float bonusElementalDamage = major.inteligence.GetValue(); // bonus elemental damage from intelligence +1 per intelligence

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if(lightningDamage > highestDamage)
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

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * 0.5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * 0.5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * 0.5f;
        //float bonusDarkness = (darknessDamage == highestDamage) ? 0 : darknessDamage * 0.5f;
        //float bonusPoison = (poisonDamage == highestDamage) ? 0 : poisonDamage * 0.5f;

        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + weakerElementDamage + bonusElementalDamage;

        return finalDamage;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.inteligence.GetValue() * 0.5f; // Bonus resistance from intelligence: +0.5% per Intelligence

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
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        float finalMaxHp = baseHp + bonusHp;

        return finalMaxHp;
    }
}
