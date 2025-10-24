using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        // Change level effect

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.5f);

        Vector3 position = GetPortalPosition(respawnType);

        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetPortalPosition(RespawnType respawnType)
    {
        var portals = FindObjectsByType<Object_Portal>(FindObjectsSortMode.None);

        foreach (var portal in portals)
        {
            portal.SetCanBeTriggered(false);
            return portal.GetPosition();
        }

        return Vector3.zero;
    }
}
