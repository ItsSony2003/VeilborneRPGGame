using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Audio_DatabaseSO audioDatabase;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    private Transform player;

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
