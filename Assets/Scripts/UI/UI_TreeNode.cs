using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;

    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;

    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#646464";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        UpdateIconColor(ConvertColorFromHex(lockedColorHex));
    }

    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);

        // find Player_ManagerSkills
        // Unlock skil on skill manager
        // skill manager unlock skill from skill data and skill type
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        return true;
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
        else
            Debug.Log("Cannot be Unlocked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTip.ShowTip(true, rect, skillData);

        if (isUnlocked == false)
            UpdateIconColor(Color.white * 0.8f);
            //Debug.Log("Show Skills TollTip");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTip.ShowTip(false, rect);

        if (isUnlocked == false)
            UpdateIconColor(lastColor);
            //Debug.Log("Hide Skills TollTip");
    }

    private Color ConvertColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }

    private void OnValidate()
    {
        if (skillData == null)
            return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.skillIcon;
        gameObject.name = "UI_TreeNode: " + skillData.skillName;
    }
}
