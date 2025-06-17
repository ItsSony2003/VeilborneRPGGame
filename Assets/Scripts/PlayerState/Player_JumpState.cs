using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rigidbody.linearVelocity.x, player.jumpForce);

    }

    public override void Update()
    {
        base.Update();

        // make sure we are not in jump attack state when we transfer to fall state
        if ((rigidbody.linearVelocity.y < 0) && (stateMachine.currentState != player.jumpAttackState))
            stateMachine.ChangeState(player.fallState);
    }
}
