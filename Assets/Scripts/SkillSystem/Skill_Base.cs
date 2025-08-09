using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed -= cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnSkillCoolDown())
        {
            Debug.LogWarning("On Cooldown");
            return false;
        }

        // mana check
        // unlock skill check

        return true;
    }

    private bool OnSkillCoolDown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetSkillCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetSkillCooldown() => lastTimeUsed = Time.time;
}
