using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPosition;

    public string lastScenePlayed;
    private bool dataLoaded;

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

    //public void GetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;

    public void ContinuePlay()
    {
        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Market";
        if (SaveManager.instance.GetGameData() == null)
            lastScenePlayed = "Market";

        ChangeScene(lastScenePlayed, RespawnType.None);
    }

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        UI_ScreenEffect fadeEffect = FindScreenEffectUI();

        // Change level effect
        FindScreenEffectUI().FadeOut(); // transparent to black
        yield return fadeEffect.fadeEffectCo;

        SceneManager.LoadScene(sceneName);

        dataLoaded = false; // data load become true when load game from save manager
        yield return null;

        while (dataLoaded == false)
        {
            yield return null;
        }

        fadeEffect = FindScreenEffectUI();
        fadeEffect.FadeIn(); // black to transparent

        Player player = Player.instance;

        if (player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private UI_ScreenEffect FindScreenEffectUI()
    {
        if (UI.instance != null)
            return UI.instance.screenEffectUI;
        else
            return FindFirstObjectByType<UI_ScreenEffect>();
    }

    private Vector3 GetNewPlayerPosition(RespawnType respawnType)
    {
        if (respawnType == RespawnType.Teleport)
        {
            Object_VeilNexus teleport = Object_VeilNexus.instance;

            Vector3 position = teleport.GetPosition();

            teleport.SetTrigger(false);
            teleport.DisableTeleportIfNeeded();

            return position;
        }

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
                .OrderBy(position => Vector3.Distance(position, lastPlayerPosition)) // arrange from closest to furthest distance
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

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Market";

        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
            return;

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    }
}
