using UnityEditor.Tilemaps;
using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // if player detecting the ground below, if yes, go to idleState
        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);

        //if (player.groundDetected && triggerCalled)
        //{
        //    float fallSpeed = Mathf.Abs(rigidbody.linearVelocity.y);
        //    if (fallSpeed < player.landSpeed)
        //    {
        //        stateMachine.ChangeState(player.landState);
        //        player.SetVelocity(0, rigidbody.linearVelocity.y);
        //    }
        //    else
        //        stateMachine.ChangeState(player.idleState);
        //}

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
