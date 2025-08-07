using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequiments;

    public override void ShowTip(bool show, RectTransform rectTransform)
    {
        base.ShowTip(show, rectTransform);
    }

    public void ShowTip(bool show, RectTransform rectTransform, Skill_DataSO skillData)
    {
        base.ShowTip(show, rectTransform);

        if (show == false)
            return;

        skillName.text = skillData.skillName;
        skillDescription.text = skillData.skillDescription;
        skillRequiments.text = "Requirements: \n"
            + " - " + skillData.skillCost + " skill point.";
    }
}
