using UnityEngine;

public class Skill_EchoOfTheLost : Skill_Base
{
    [SerializeField] private GameObject echoOfTheLostPrefab;
    [SerializeField] private float cloneDuration;

    [Header("Clone Attack Upgrade")]
    [SerializeField] private int maxCloneAttacks = 3;
    [SerializeField] private float duplicateChance = 0.25f;

    [Header("Heal Remnant upgrade")]
    [SerializeField] private float damagePercentHealed = 0.25f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if (CanBeRemnant() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCooldownReducedInSeconds()
    {
        if (upgradeType != SkillUpgradeType.EchoClone_CooldownRemnant)
            return 0;

        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffect()
    {
        return upgradeType == SkillUpgradeType.EchoClone_CleanseRemnant;
    }

    public bool CanBeRemnant()
    {
        return upgradeType == SkillUpgradeType.EchoClone_HealRemnant
            || upgradeType == SkillUpgradeType.EchoClone_CleanseRemnant
            || upgradeType == SkillUpgradeType.EchoClone_CooldownRemnant;
    }

    public float GetCloneDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.EchoClone_ChanceToDuplicate)
            return 0;

        return duplicateChance;
    }

    public int GetMaxCloneAttacks()
    {
        if (upgradeType == SkillUpgradeType.EchoClone_SingleAttack || upgradeType == SkillUpgradeType.EchoClone_ChanceToDuplicate)
            return 1;

        if (upgradeType == SkillUpgradeType.EchoClone_MultiAttack)
            return maxCloneAttacks;

        return 0;
    }

    public float GetCloneDuration()
    {
        return cloneDuration;
    }

    public override void TryToUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreateClone();
    }

    public void CreateClone(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject echo = Instantiate(echoOfTheLostPrefab, position, Quaternion.identity);
        
        // Setup echo of the lost
        echo.GetComponent<SkillObject_EchoOfTheLost>().SetUpEcho(this);
    }
}
