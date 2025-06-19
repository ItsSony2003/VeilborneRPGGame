using UnityEngine;

public class Player_RunState : Player_GroundState
{
    public Player_RunState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        // If stop moving, go idle
        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.moveInput.x * player.runSpeed, rb.linearVelocity.y);
    }
}
