using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            Die();
    }

    protected override void Die()
    {
        base.Die();

        player.ui.OpenDeathUI();
        //GameManager.instance.GetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();

        // trigger death UI
    }
}
