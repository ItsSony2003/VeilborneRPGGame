using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectionDetails
{
    public UI_TreeConnectionHandler childNode;
    // Direction of connection
    public TreeNodeDirectionType direction;
    [Range(100f, 400f)] public float length;
    [Range(-35f, 35f)] public float rotation;
}
public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectionDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originialColor;

    private void Awake()
    {
        if (connectionImage != null)
            originialColor = connectionImage.color;
    }

    public UI_TreeNode[] GetAllChildNodes()
    {
        List<UI_TreeNode> childNodeToReturn = new List<UI_TreeNode>();

        foreach (var node in details)
        {
            if (node.childNode != null)
                childNodeToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
        }

        return childNodeToReturn.ToArray();
    }

    public void UpdateTreeConnections()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];

            // connected child node position = detail.getConnectinPoint
            Vector2 targetPosition = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);
            
            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateTreeConnections();

        foreach (var node in details)
        {
            if (node.childNode == null)
                continue;

            node.childNode.UpdateTreeConnections();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originialColor;
    }

    public void SetConnectionImage(Image image) => connectionImage = image;

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;

    private void OnValidate()
    {
        if (details.Length <= 0)
            return;

        if (details.Length != connections.Length)
        {
            Debug.Log("Amount of details should be the same as amount of connections. - " + gameObject.name);
            return;
        }

        UpdateTreeConnections();
    }
}
