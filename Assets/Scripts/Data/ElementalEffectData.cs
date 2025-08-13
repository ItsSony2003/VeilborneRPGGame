using System;

public class ElementalEffectData
{
    public float slowDuration;
    public float slowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float lightningDuration;
    public float lightningDamage;
    public float lightningCharge;

    public ElementalEffectData(Entity_Stats entityStats, DamageScaleData damageScale)
    {
        slowDuration = damageScale.slowDuration;
        slowMultiplier = damageScale.slowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;

        lightningDuration = damageScale.lightningDuration;
        lightningDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.lightningDamageScale;
        lightningCharge = damageScale.lightningCharge;
    }
}
