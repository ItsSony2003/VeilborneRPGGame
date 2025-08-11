using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float ignitionTime = 2.5f;

    public void CreateShard()
    {
        if (upgradeType == SkillUpgradeType.None)
            return;

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetUpShardExplosion(ignitionTime);
    }
}
