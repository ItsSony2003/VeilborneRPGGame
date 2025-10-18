using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "veilborne.json";
    [SerializeField] private bool encryptData = true;

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindSaveables();

        yield return new WaitForSeconds(0.01f);
        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save game yet, creating new save now!");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    public void SaveGame()
    {
        foreach (var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    [ContextMenu("*** DELELTE SAVE DATA ***")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindSaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}
