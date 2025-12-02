using UnityEngine;

public class Player_Health : Entity_Health, ISaveable
{
    private Player player;

    private bool pendingRestore;
    private float savedPercent = 1f;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //    Die();
    }

    protected override void Die()
    {
        base.Die();

        player.ui.OpenDeathUI();
        //GameManager.instance.GetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();
    }

    public void SaveData(ref GameData data)
    {
        data.currentHealthPercent = GetHealthPercent();
    }

    public void LoadData(GameData data)
    {
        savedPercent = (data.currentHealthPercent <= 0f) ? 1f : data.currentHealthPercent;
        pendingRestore = true;
    }

    private void LateUpdate()
    {
        if (pendingRestore)
        {
            ConvertHealthToPercent(savedPercent);
            pendingRestore = false;
        }
    }
}
