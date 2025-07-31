using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Entity Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    //public float poisonDamage;
    //public float darknessDamage;

    [Header("Defense - Physical")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;
    //public float poisonResistance;
    //public float darknessResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
