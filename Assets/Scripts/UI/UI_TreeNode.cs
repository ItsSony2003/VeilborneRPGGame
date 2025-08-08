using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectionHandler treeConnectionHandler;

    [Header("Unlock Details")]
    public UI_TreeNode[] requiredNodes; // skills required to unlock this one
    public UI_TreeNode[] conflictedNodes; // skill lock if choose the other path
    public bool isUnlocked;
    public bool isLocked;


    [Header("Skill Details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#646464";
    private Color lastColor;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        treeConnectionHandler = GetComponent<UI_TreeConnectionHandler>();

        UpdateIconColor(ConvertColorFromHex(lockedColorHex));
    }

    public void Refund()
    {
        isLocked = false;
        isUnlocked = false;
        UpdateIconColor(ConvertColorFromHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.skillCost);
        treeConnectionHandler.UnlockConnectionImage(false);

        // skill manager and reset all skill
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictedNodes();

        skillTree.ReduceSkillPoints(skillData.skillCost);
        treeConnectionHandler.UnlockConnectionImage(true);

        // find Player_ManagerSkills
        // Unlock skil on skill manager
        // skill manager unlock skill from skill data and skill type
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (skillTree.EnoughSkillPoints(skillData.skillCost) == false)
            return false;

        foreach (var node in requiredNodes)
        {
            if (node.isUnlocked == false) 
                return false;
        }

        foreach (var node in conflictedNodes)
        {
            if (node.isUnlocked) 
                return false;
        }

        return true;
    }

    private void LockConflictedNodes()
    {
        foreach (var node in conflictedNodes)
            node.isLocked = true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            ui.skillToolTip.LockedSkillEffect();
            //Debug.Log("Cannot be Unlocked");
    }

    //"Show Skills TollTip
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowTip(true, rect, this);

        if (isUnlocked == false || isLocked == false)
            ToggleNodeHighlight(true);
    }

    // Hide Skills TollTip
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);

        if (isUnlocked == false || isLocked == false)
            ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * 0.8f; highlightColor.a = 1;
        Color appliedColor = highlight ? highlightColor : lastColor;

        UpdateIconColor(appliedColor);
    }

    private Color ConvertColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(ConvertColorFromHex(lockedColorHex));

        if (isUnlocked)
            UpdateIconColor(Color.white);
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.skillIcon;
        skillCost = skillData.skillCost;
        gameObject.name = "UI_TreeNode: " + skillData.skillName;
    }
}
