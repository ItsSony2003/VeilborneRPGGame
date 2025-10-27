using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 lastDeathPosition;

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

    public void GetLastDeathPosition(Vector3 position) => lastDeathPosition = position;

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
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

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(RespawnType respawnType)
    {
        if (respawnType == RespawnType.None)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var enteredPortals = FindObjectsByType<Object_Portal>(FindObjectsSortMode.None)
                .Where(p => p.GetRespawnType() == RespawnType.Enter)
                .Select(p => p.GetPositionAndSetTriggerFalse())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enteredPortals).ToList(); // combine 2 list into 1 list

            if (selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions
                .OrderBy(position => Vector3.Distance(position, lastDeathPosition)) // arrange from closest to furthest distance
                .First();
        }

        return GetPortalPosition(respawnType);
    }

    private Vector3 GetPortalPosition(RespawnType respawnType)
    {
        var portals = FindObjectsByType<Object_Portal>(FindObjectsSortMode.None);

        foreach (var portal in portals)
        {
            if (portal.GetRespawnType() == respawnType)
                return portal.GetPositionAndSetTriggerFalse();
        }

        return Vector3.zero;
    }
}
