using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Get Skill Point", fileName = "Item Effect Data - Get Skill Point")]
public class ItemEffect_GetSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}
