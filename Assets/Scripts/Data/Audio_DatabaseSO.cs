using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class Audio_DatabaseSO : ScriptableObject
{
    public List<AudioClipData> playerAudio;
    public List<AudioClipData> uiAudio;

    [Header("Music Playlists")]
    public List<AudioClipData> mainMenuMusic;
    public List<AudioClipData> levelMusic;
    public List<AudioClipData> bossMusic;

    private Dictionary<string, AudioClipData> audioClipCollection;

    private void OnEnable()
    {
        audioClipCollection = new Dictionary<string, AudioClipData>();

        AddToCollection(playerAudio);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
        AddToCollection(bossMusic);
    }

    public AudioClipData GetAudio(string groupName)
    {
        return audioClipCollection.TryGetValue(groupName, out var data) ? data : null;
    }

    private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach (var data in listToAdd)
        {
            if (data != null && audioClipCollection.ContainsKey(data.audioName) == false)
            {
                audioClipCollection.Add(data.audioName, data);
            }
        }
    }
}

[System.Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0f, 1f)] public float maxVolume = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Count == 0)
            return null;

        return clips[Random.Range(0, clips.Count)];
    }
}
