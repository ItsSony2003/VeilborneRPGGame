using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;
    public Stat_MajorStats major;
    public Stat_OffenseStats offense;
    public Stat_DefenseStats defense;

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        
        return baseHp + bonusHp;
    }
}
