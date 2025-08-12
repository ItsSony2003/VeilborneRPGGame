using System;
using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float ignitionTime = 2.5f;

    [Header("Detect Shard Upgrade Details")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multiple Shard Upgrade Details")]
    [SerializeField] private int maxShard = 3;
    [SerializeField] private int currentShardAmount;
    [SerializeField] private bool isRegenerating;

    [Header("Telport Shard Upgrade Details")]
    [SerializeField] private float shardExistDuration = 12;

    [Header("Teleport + Hp Rewind Shard Upgrade Details")]
    [SerializeField] private float savedHealthPercent;


    protected override void Awake()
    {
        base.Awake();

        currentShardAmount = maxShard;
        playerHealth = GetComponentInParent<Entity_Health>();
    }

    public override void TryToUseSkill()
    {
        base.TryToUseSkill();

        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_DetectEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_MultiShard))
            HandleShardMultipleCast();

        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if (Unlocked(SkillUpgradeType.Shard_TeleportAndHealthRewind))
            HandleShardTeleportHealthRewind();
    }

    private void HandleShardTeleportHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShardPosition();
            playerHealth.ConvertHealthToPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
            CreateShard();
        else
        {
            SwapPlayerAndShardPosition();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShardPosition()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }

    private void HandleShardMultipleCast()
    {
        if (currentShardAmount <= 0) 
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentShardAmount--;

        if (isRegenerating == false)
            StartCoroutine(ShardCreateCo());
    }

    private IEnumerator ShardCreateCo()
    {
        isRegenerating = true;

        while (currentShardAmount < maxShard)
        {
            yield return new WaitForSeconds(cooldown);
            currentShardAmount++;
        }

        isRegenerating = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);

        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        float ignitionTime = GetIgnitionTime();

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetUpShardTime(ignitionTime);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHealthRewind))
            currentShard.OnExplode += ForceCoolDown;
    }

    public float GetIgnitionTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHealthRewind))
            return shardExistDuration;

        return ignitionTime;
    }

    private void ForceCoolDown()
    {
        if(OnSkillCoolDown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCoolDown;
        }
    }
}
