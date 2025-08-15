using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash {  get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.WraithcoreShard: return shard;
            case SkillType.RiftfangSwordThrow: return swordThrow;

            default:
                Debug.Log($"SKill type {type} is not implemented yet!");
                return null;
        }
    }
}
