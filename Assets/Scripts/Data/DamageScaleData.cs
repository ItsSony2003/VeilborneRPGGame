using System;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage Details")]
    public float physical = 1;
    public float elemental = 1;

    [Header("Slow")]
    public float slowDuration = 3;
    public float slowMultiplier = 0.25f;

    [Header("Burn")]
    public float burnDuration = 3;
    public float burnDamageScale = 1;

    [Header("Lightning")]
    public float lightningDuration = 3;
    public float lightningDamageScale = 1;
    public float lightningCharge = 0.4f;
}
