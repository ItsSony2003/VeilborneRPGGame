using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Audio_DatabaseSO audioDatabase;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

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

    public void PlaySFX(string soundName, AudioSource sfxSource)
    {
        var data = audioDatabase.GetAudio(soundName);
        if (data == null)
        {
            Debug.Log("attempt to play sound - " + soundName);
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null)
            return;

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.volume = data.volume;
        sfxSource.PlayOneShot(clip);
    }
}
