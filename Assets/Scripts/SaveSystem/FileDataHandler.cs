using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "veilbornerpggame";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool encryptData)
    {
        fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        this.encryptData = encryptData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            // 1. Create directory if doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // 2. Convert GameData to JSON string
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if (encryptData)
                dataToSave = EncryptDecrypt(dataToSave);

            // 3. Open or Create data
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // 4. Write JSON info into the file
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }

        catch (Exception e)
        {
            // Log any error that happens
            Debug.LogError("Error trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        // 1. Check if any save file exists
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                // 2. Open the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // 3. Read file's text content
                    using (StreamReader read = new StreamReader(stream))
                    {
                        dataToLoad = read.ReadToEnd();
                    }
                }

                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // 4. Convert the JSON string back into a GameData Object
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch(Exception e)
            {
                Debug.LogError("Error trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void Delete()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    // Scramble Data so human can't read it, when we run function again then unscrambles
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}
