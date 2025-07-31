using System.Collections;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Entity_Stats statsToModify;

    [Header("Buff Details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private bool canBeApplied = true;

    [Header("Y Floating Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatAmplitude = 0.3f;
    private Vector3 startPosition;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        if (canBeApplied == false) return;

        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeApplied = false;
        spriteRenderer.color = Color.clear;
        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        ApplyBuff(false);
        Destroy(gameObject);
    }    

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
                //Debug.Log("Buff is applied for: " + duration);
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                //Debug.Log("Buff is removed!");
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
        
    }
}
