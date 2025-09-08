using System;
using UnityEngine;

[Serializable]
public class Stat_DefenseStats
{
    // Physical defense
    public Stat armor;
    public Stat evasion; // chance to fully dodge attack

    // Elemental Resistance
    public Stat fireRes;
    public Stat iceRes;
    public Stat lightningRes;
}
