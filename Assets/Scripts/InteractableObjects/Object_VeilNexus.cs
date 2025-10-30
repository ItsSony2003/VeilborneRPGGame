using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_VeilNexus : MonoBehaviour, ISaveable
{
    public static Object_VeilNexus instance;

    public bool isActive { get; private set; }
    [SerializeField] private Vector2 defaultPosition; // where the portal spawns in town
    [SerializeField] private string marketSceneName = "Market";

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered;

    private string currentSceneName;
    private string returnSceneName;
    private bool returnFromMarket;

    private void Awake()
    {
        instance = this;
        currentSceneName = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999); // hide when not use it
    }

    public void ActivateTeleport(Vector3 position, int facingDirection = 1)
    {
        isActive = true;
        transform.position = position;
        SaveManager.instance.GetGameData().inSceneTeleports.Clear();

        if (facingDirection == -1)
            transform.Rotate(0, 180, 0);
    }

    public void DisableTeleportIfNeeded()
    {
        if (returnFromMarket == false)
            return;

        SaveManager.instance.GetGameData().inSceneTeleports.Remove(currentSceneName);
        isActive = false;
        transform.position = new Vector3(9999, 9999);
    }

    private void UseTeleport()
    {
        // TRANSFER TO A DIFFERENT SCENE
        string destinationScene = InMarket() ? returnSceneName : marketSceneName;

        GameManager.instance.ChangeScene(destinationScene, RespawnType.Teleport);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
            return;

        UseTeleport();
    }

    private void OnTriggerExit2D(Collider2D collision) => canBeTriggered = true;

    public void SetTrigger(bool trigger) => canBeTriggered = trigger;

    public Vector3 GetPosition() => respawnPoint != null ? respawnPoint.position: transform.position;
    private bool InMarket() => currentSceneName == marketSceneName;

    public void LoadData(GameData data)
    {
        if (InMarket() && data.inSceneTeleports.Count > 0)
        {
            transform.position = defaultPosition;
            isActive = true;
        }
        else if (data.inSceneTeleports.TryGetValue(currentSceneName, out Vector3 teleportPosition))
        {
            transform.position = teleportPosition;
            isActive = true;
        }

        returnFromMarket = data.returningFromMarket;
        returnSceneName = data.teleportDestinationSceneName;
    }

    public void SaveData(ref GameData data)
    {
        data.returningFromMarket = InMarket();

        if (isActive && InMarket() == false)
        {
            data.inSceneTeleports[currentSceneName] = transform.position;
            data.teleportDestinationSceneName = currentSceneName;
        }
        else
        {
            data.inSceneTeleports.Remove(currentSceneName);
        }
    }
}
