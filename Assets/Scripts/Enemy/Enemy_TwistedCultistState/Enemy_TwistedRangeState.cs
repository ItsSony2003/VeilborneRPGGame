using UnityEngine;

public class Enemy_TwistedRangeState : EnemyState
{
    private Enemy_TwistedCultist twistedCultist;

    public Enemy_TwistedRangeState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        twistedCultist = enemy as Enemy_TwistedCultist;
    }

    public override void Enter()
    {
        base.Enter();

        twistedCultist.SetVelocity(0, 0);
        twistedCultist.SetRangeCastPerformed(false);
        enemy.gameObject.layer = LayerMask.NameToLayer("Untouchable");
    }

    public override void Update()
    {
        base.Update();

        if (twistedCultist.rangeCastPerform)
            anim.SetBool("rangeCast_performed", true);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("rangeCast_performed", false);
        enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
