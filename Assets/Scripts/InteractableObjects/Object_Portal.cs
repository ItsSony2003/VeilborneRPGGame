using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour
{
    [SerializeField] private string transferToScene;

    [Space]
    [SerializeField] private RespawnType respawnType;
    [SerializeField] private RespawnType connectedPortal;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered = true;

    public RespawnType GetRespawnType() => respawnType;

    public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    private void OnValidate()
    {
        gameObject.name = "Object_Portal - " + respawnType.ToString() + " - " + transferToScene;

        if (respawnType == RespawnType.Enter)
            connectedPortal = RespawnType.Exit;

        if (respawnType == RespawnType.Exit)
            connectedPortal = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
            return;

        // transfer to another level
        GameManager.instance.ChangeScene(transferToScene, connectedPortal);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
