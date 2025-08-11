using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }

    private void CreateShard()
    {
        Debug.Log("Create Time Shard");

        // sklill nmanager shard create
    }

    private void CreateClone()
    {
        Debug.Log("Create Clone Echo");

        // sklill nmanager clone create
    }
}
