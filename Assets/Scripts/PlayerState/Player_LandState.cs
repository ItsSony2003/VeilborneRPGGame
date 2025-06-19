using UnityEngine;

public class Player_LandState : PlayerState
{
    public Player_LandState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    //public override void Enter()
    //{
    //    base.Enter();
    //}

    //public override void Update()
    //{
    //    base.Update();
        
    //    // attack and damage enemy
    //    if (triggerCalled)
    //        stateMachine.ChangeState(player.idleState);
    //}
}
