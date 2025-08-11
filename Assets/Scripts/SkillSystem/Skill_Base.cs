using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;

        if (OnSkillCoolDown())
        {
            Debug.LogWarning("On Cooldown");
            return false;
        }

        // mana check
        // unlock skill check

        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    private bool OnSkillCoolDown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetSkillCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetSkillCooldown() => lastTimeUsed = Time.time;
}
