using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private Player_Stats statsToModify;

    [Header("Buff Details")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5f;

    [Header("Y Floating Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatAmplitude = 0.3f;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // start coroutine with buiff effect to start and end buff
        statsToModify = collision.GetComponent<Player_Stats>();

        if (statsToModify.CanApplyBuffOf(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}
