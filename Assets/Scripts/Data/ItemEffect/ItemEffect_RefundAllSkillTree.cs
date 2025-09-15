using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Refund All Skills Effect", fileName = "Item Effect Data - Refund All Skills")]
public class ItemEffect_RefundAllSkillTree : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
