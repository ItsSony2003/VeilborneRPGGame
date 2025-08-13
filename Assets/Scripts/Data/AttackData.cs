using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entityStats, DamageScaleData damageScaleData)
    {
        physicalDamage = entityStats.GetPhysicalDamage(out isCrit, damageScaleData.physical);
        elementalDamage = entityStats.GetElementalDamage(out element, damageScaleData.elemental);

        effectData = new ElementalEffectData(entityStats, damageScaleData);
    }
}
