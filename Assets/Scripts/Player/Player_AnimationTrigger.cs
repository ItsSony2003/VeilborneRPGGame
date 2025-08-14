using UnityEngine;

public class Player_AnimationTrigger : Entity_AnimationTrigger
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    private void SwordThrow() => player.skillManager.swordThrow.SwordThrow();
}
