using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(bool show, RectTransform rectTransform, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, rectTransform);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemDescription.text = GetItemDescription(itemToShow);
    }

    public string GetItemDescription(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return "Used for Creafing!";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var modifier in item.modifiers)
        {
            string modifierType = GetStatNameByType(modifier.statType);
            string modifierValue = IsPercentageStat(modifier.statType) ? modifier.value.ToString() + "%" : modifier.value.ToString();

            sb.AppendLine("+ " + modifierValue + " " + modifierType);
        }

        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Attack Damage";
            case StatType.CritChance: return "Crit Chance";
            case StatType.CritPower: return "Crit Power";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";

            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.FireResistance: return "Fire Resistance";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default:
                return "Unknown Stat";
        }
    }

    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.AttackSpeed:
            case StatType.Evasion:
            case StatType.FireResistance:
            case StatType.IceResistance:
            case StatType.LightningResistance:
                return true;
            default:
                return false;
        }
    }    
}
