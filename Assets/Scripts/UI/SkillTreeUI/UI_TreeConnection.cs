using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(TreeNodeDirectionType direction, float length, float offset)
    {
        bool shouldBeActive = direction != TreeNodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform,
                childNodeConnectionPoint.position,
                null,
                out var localPosition
            );

        return localPosition;
    }

    private float GetDirectionAngle(TreeNodeDirectionType type)
    {
        switch(type)
        {
            case TreeNodeDirectionType.UpLeft: return 135f;
            case TreeNodeDirectionType.Up: return 90f;
            case TreeNodeDirectionType.UpRight: return 45f;
            case TreeNodeDirectionType.Left: return 180f;
            case TreeNodeDirectionType.Right: return 0f;
            case TreeNodeDirectionType.DownLeft: return -135f;
            case TreeNodeDirectionType.Down: return -90f;
            case TreeNodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }
}

public enum TreeNodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}