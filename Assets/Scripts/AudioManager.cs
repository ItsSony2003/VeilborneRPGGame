using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Audio_DatabaseSO audioDatabase;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [Space]
    private Transform player;

    private AudioClip lastMusicPlayed;
    private string currentBgmPlaylistName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;

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

    private void Update()
    {
        if (bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if (string.IsNullOrEmpty(currentBgmPlaylistName) == false)
                NextBGM(currentBgmPlaylistName);
        }

        if (bgmSource.isPlaying && bgmShouldPlay == false)
            StopBGM();
    }

    public void StartBGM(string playlistGroup)
    {
        bgmShouldPlay = true;

        if (playlistGroup == currentBgmPlaylistName)
            return;

        NextBGM(playlistGroup);
    }

    public void NextBGM(string playlistGroup)
    {
        bgmShouldPlay = true;
        currentBgmPlaylistName = playlistGroup;

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);

        currentBgmCo = StartCoroutine(SwitchMusicCo(playlistGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCo(bgmSource, 0, 2f));

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);
    }

    private IEnumerator SwitchMusicCo(string playlistGroup)
    {
        AudioClipData data = audioDatabase.GetAudio(playlistGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if (data == null || data.clips.Count == 0)
        {
            Debug.Log(" No Music in this playlist " + playlistGroup);
            yield break;
        }

        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        }

        if (bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0, 2f);

        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.volume = 0;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 2f));
    }

    private IEnumerator FadeVolumeCo(AudioSource audioSource, float targetVolume, float duration)
    {
        float time = 0;
        float startVolume = audioSource.volume;

        while (time < duration)
        {
            time += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSFX = 7)
    {
        if (player == null)
            player = Player.instance.transform;

        var data = audioDatabase.GetAudio(soundName);
        if (data == null)
        {
            Debug.Log("attempt to play sound - " + soundName);
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null)
            return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float r = Mathf.Clamp01(1- (distance / minDistanceToHearSFX));

        //sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.volume = Mathf.Lerp(0, maxVolume, r * r); // exponential falloff
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDatabase.GetAudio(soundName);
        if (data == null)
            return;

        var clip = data.GetRandomClip();
        if (clip == null)
            return;

        Debug.Log("Play Audio " + soundName);
        //sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
