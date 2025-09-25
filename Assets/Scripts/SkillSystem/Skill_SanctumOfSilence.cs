using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_SanctumOfSilence : Skill_Base
{
    [SerializeField] private GameObject sanctumPrefab;

    [Header("Slow Down Upgrade Details")]
    [SerializeField] private float slowSanctumPercent = 0.85f;
    [SerializeField] private float slowSanctumDuration = 5;

    [Header("Shard Casting Upgrade Details")]
    [SerializeField] private int shardToCast = 16;
    [SerializeField] private float shardCastSanctumSlow = 1;
    [SerializeField] private float shardCastSanctumDuration = 8;
    private float spellTimer;
    private float spellPerSecond;

    [Header("Clone Casting Upgrade Details")]
    [SerializeField] private int cloneToCast = 16;
    [SerializeField] private float cloneCastSanctumSlow = 1;
    [SerializeField] private float cloneCastSanctumDuration = 6;
    //[SerializeField] private float healthRestore = 0.08f;

    [Header("Domain Details")]
    public float maxSanctumSize = 15;
    public float expandSanctumSpeed = 2.5f;

    public List<Enemy> insideTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateSanctum()
    {
        spellPerSecond = GetSpellToCast() / GetSanctumDuration();

        GameObject sanctum = Instantiate(sanctumPrefab, transform.position, Quaternion.identity);
        sanctum.GetComponent<SkillObject_SanctumOfSilence>().SetUpSanctum(this);
    }

    public void DoSpellCasting()
    {
        spellTimer -= Time.deltaTime;

        if (currentTarget == null )
            currentTarget = FindTargetInSanctum();

        if (currentTarget != null && spellTimer < 0)
        {
            // cast spell
            CastSpell(currentTarget);
            spellTimer = 1 / spellPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Sanctum_MultiClone)
        {
            Vector3 offset = Random.value < 0.6f ? new Vector2(2, 0) : new Vector2(-2, 0);

            skillManager.echoOfTheLost.CreateClone(target.position + offset);
        }

        if (upgradeType == SkillUpgradeType.Sanctum_MultiShard)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInSanctum()
    {
        insideTargets.RemoveAll(target => target == null || target.health.isDead);

        if (insideTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, insideTargets.Count);
        return insideTargets[randomIndex].transform;
    }

    public float GetSanctumDuration()
    {
        if (upgradeType == SkillUpgradeType.Sanctum_SlowDown)
            return slowSanctumDuration;
        else if (upgradeType == SkillUpgradeType.Sanctum_MultiShard)
            return shardCastSanctumDuration;
        else if (upgradeType == SkillUpgradeType.Sanctum_MultiClone)
            return cloneCastSanctumDuration;

        return 0;
    }

    public float GetSlowPercent()
    {
        if (upgradeType == SkillUpgradeType.Sanctum_SlowDown)
            return slowSanctumPercent;
        else if (upgradeType == SkillUpgradeType.Sanctum_MultiShard)
            return shardCastSanctumSlow;
        else if (upgradeType == SkillUpgradeType.Sanctum_MultiClone)
            return cloneCastSanctumSlow;

        return 0;
    }

    private int GetSpellToCast()
    {
        if (upgradeType == SkillUpgradeType.Sanctum_MultiShard)
            return shardToCast;
        else if (upgradeType == SkillUpgradeType.Sanctum_MultiClone)
            return cloneToCast;

        return 0;
    }

    public bool InstantSanctum()
    {
        return upgradeType != SkillUpgradeType.Sanctum_MultiShard
            && upgradeType != SkillUpgradeType.Sanctum_MultiClone;
    }

    public void AddInsideTarget(Enemy targetToAdd)
    {
        insideTargets.Add(targetToAdd);
    }

    public void ClearInsideTargets()
    {
        foreach (var enemy in insideTargets)
            enemy.StopSlowDown();

        insideTargets = new List<Enemy>();
    }
}
