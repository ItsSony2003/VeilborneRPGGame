using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int skillCost;

    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string skillDescription;
    public Sprite skillIcon;

    // skill type that you should unlock
}
