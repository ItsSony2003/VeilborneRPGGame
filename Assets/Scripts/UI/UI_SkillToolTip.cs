using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCooldown;
    [SerializeField] private TextMeshProUGUI skillRequiments;

    [Space]
    [SerializeField] private string meetRequirementHex;
    [SerializeField] private string notMeetRequirementHex;
    [SerializeField] private string conflictHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "Another path has been chosen - this path is locked";

    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
        // true to make sure game object is active even if it is disable in hierachy
    }

    public override void ShowToolTip(bool show, RectTransform rectTransform)
    {
        base.ShowToolTip(show, rectTransform);
    }

    public void ShowTip(bool show, RectTransform rectTransform, UI_TreeNode node)
    {
        base.ShowToolTip(show, rectTransform);

        if (show == false)
            return;

        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.skillDescription;
        skillCooldown.text = "Cooldonw: " + node.skillData.upgradeData.cooldown + " second(s).";

        string lockedText = GetColoredText(conflictHex, lockedSkillText);
        string requirements = node.isLocked ? lockedText : GetSkillRequirements(node.skillData.skillCost, node.requiredNodes, node.conflictedNodes);

        skillRequiments.text = requirements;
    }

    public void LockedSkillEffect()
    {
        if (textEffectCo != null)
            StopCoroutine(textEffectCo);

        textEffectCo = StartCoroutine(TextBlinkingEffectCo(skillRequiments, 0.2f, 3));
    }

    private IEnumerator TextBlinkingEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMeetRequirementHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColoredText(conflictHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private string GetSkillRequirements(int skillCost, UI_TreeNode[] requiredNodes, UI_TreeNode[] conflictedNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Skill Requirements:");

        string costColor = skillTree.EnoughSkillPoints(skillCost) ? meetRequirementHex : notMeetRequirementHex;
        string costText = $"- {skillCost} skill point(s)";
        string finalCostText = GetColoredText(costColor, costText);

        sb.AppendLine(finalCostText);

        foreach (var node in requiredNodes)
        {
            if (node == null) continue;

            string nodeColor = node.isUnlocked ? meetRequirementHex : notMeetRequirementHex;
            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColoredText(nodeColor, nodeText);

            sb.AppendLine(finalNodeText);
        }

        if (conflictedNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine(); // Spacing 1 line
        sb.AppendLine(GetColoredText(conflictHex, "Will be locked: "));

        foreach (var node in conflictedNodes)
        {
            if (node == null) continue;

            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColoredText(conflictHex, nodeText);

            sb.AppendLine(finalNodeText);
        }

        return sb.ToString();
    }
}
