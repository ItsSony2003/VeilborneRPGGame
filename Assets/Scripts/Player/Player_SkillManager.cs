using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash {  get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_EchoOfTheLost echoOfTheLost { get; private set; }
    public Skill_SanctumOfSilence sanctumOfSilence { get; private set; }

    public Skill_Base[] allSkills { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        echoOfTheLost = GetComponentInChildren<Skill_EchoOfTheLost>();
        sanctumOfSilence = GetComponentInChildren<Skill_SanctumOfSilence>();

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceSkillCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.WraithcoreShard: return shard;
            case SkillType.RiftfangSwordThrow: return swordThrow;
            case SkillType.EchoClone: return echoOfTheLost;
            case SkillType.SanctumOfSilence: return sanctumOfSilence;

            default:
                Debug.Log($"SKill type {type} is not implemented yet!");
                return null;
        }
    }
}
