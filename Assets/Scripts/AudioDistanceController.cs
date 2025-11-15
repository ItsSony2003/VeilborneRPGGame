using UnityEngine;

public class AudioDistanceController : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform player;

    [SerializeField] private float minDistanceToHearSound = 25f;
    [SerializeField] private bool showGizmo;
    private float maxVolume;

    private void Start()
    {
        player = Player.instance.transform;
        audioSource = GetComponent<AudioSource>();

        maxVolume = audioSource.volume;
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(player.position, transform.position);
        float r = Mathf.Clamp01(1 - (distance / minDistanceToHearSound));
        float targetVolume = Mathf.Lerp(0, maxVolume, r * r);
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * 2);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, minDistanceToHearSound);
        }
    }
}
