using UnityEngine;

public class Enemy_TwistedBattleState : Enemy_BattleState
{
    private Enemy_TwistedCultist twistedCultist;
    //private float lastTimeUseRetreat = float.NegativeInfinity;
    private float lastTimeUseRetreat = 0f;
    private bool hasInitializedCooldown = false;

    public Enemy_TwistedBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        twistedCultist = enemy as Enemy_TwistedCultist;
    }

    public override void Enter()
    {
        base.Enter();

        //if (ShouldRetreat())
        //{
        //    if (CanUseRetreatAbility())
        //        EnterRetreatState();
        //    else
        //        ShortRetreat();
        //}
        // Initialize cooldown on first ever entry, so retreat isn't available immediately
        if (!hasInitializedCooldown)
        {
            lastTimeUseRetreat = Time.time; // ability locked until cooldown passes
            hasInitializedCooldown = true;
        }

        if (ShouldRetreat())
        {
            if (CanUseRetreatAbility())
                EnterRetreatState();
            else
                ShortRetreat();
        }
    }

    private void EnterRetreatState()
    {
        lastTimeUseRetreat = Time.time;

        stateMachine.ChangeState(twistedCultist.twistedRetreatState);
    }

    private bool CanUseRetreatAbility() => Time.time > lastTimeUseRetreat + twistedCultist.retreatCooldown;
}
