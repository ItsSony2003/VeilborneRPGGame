using System;
using UnityEngine;

[Serializable]
public class UI_TreeConnectionDetails
{
    public UI_TreeConnectionHandler childNode;
    // Direction of connection
    public TreeNodeDirectionType direction;
    [Range(100f, 400f)] public float length;
}
public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private UI_TreeConnectionDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (details.Length != connections.Length)
        {
            Debug.Log("Amount of details should be the same as amount of connections. - " + gameObject.name);
            return;
        }

        UpdateTreeConnections();
    }

    private void UpdateTreeConnections()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            // connected child node position = detail.getConnectinPoint
            Vector2 targetPosition = connection.GetConnectionPoint(rect);

            connection.DirectConnection(detail.direction, detail.length);
            detail.childNode.SetPosition(targetPosition);
        }
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
