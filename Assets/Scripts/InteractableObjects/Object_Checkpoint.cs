using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Object_Checkpoint[] checkpoints;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }

    public void ActivateCheckpoint(bool activate)
    {
        anim.SetBool("isActive", activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var checkpoint in checkpoints)
            checkpoint.ActivateCheckpoint(false);

        // Save position to save manager
        SaveManager.instance.GetGameData().savedCheckpoint = transform.position;
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckpoint == transform.position;
        ActivateCheckpoint(active);

        if (active)
            Player.instance.TeleportPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
