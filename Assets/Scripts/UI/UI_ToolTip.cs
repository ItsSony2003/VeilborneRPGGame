using UnityEngine;
using System.Data;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(300, 30);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform rectTransform)
    {
        if (show == false)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }

        UpdateTipPosition(rectTransform);
    }

    private void UpdateTipPosition(RectTransform rectTransform)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0;

        Vector2 targetPosition = rectTransform.position;

        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offset.x : targetPosition.x + offset.x;

        float tipHalfHeight = rect.sizeDelta.y / 2;
        float topHeight = targetPosition.y + tipHalfHeight;
        float bottomHeight = targetPosition.y - tipHalfHeight;

        if (topHeight > screenTop)
            targetPosition.y = screenTop - tipHalfHeight - offset.y;
        else if (bottomHeight < screenBottom)
            targetPosition.y = screenBottom + tipHalfHeight + offset.y;

        rect.position = targetPosition;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}
