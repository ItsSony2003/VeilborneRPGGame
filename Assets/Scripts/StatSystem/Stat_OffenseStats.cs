using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseStats
{
    // Physical damage
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    // Elemental damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    //public Stat darknessDamage;
    //public Stat poisonDamage;
}
