using UnityEngine;

public class Enemy_MageCultistBattleState : Enemy_BattleState
{
    private bool canFlip;
    private bool reachedDeadend;

    public Enemy_MageCultistBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        reachedDeadend = false;
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (enemy.groundDetected == false || enemy.wallDetected)
            reachedDeadend = true;

        if (enemy.PlayerDectection())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (CanAttack())
        {
            if (enemy.PlayerDectection() == false && canFlip)
            {
                enemy.HandleFlip(DirectionToPlayer());
                canFlip = false;
            }

            enemy.SetVelocity(0, rb.linearVelocity.y);

            if (WithinAttackRange() && enemy.PlayerDectection())
            {
                canFlip = true;
                lastTimeAttack = Time.time;
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            // walks away from player
            bool walkAway = reachedDeadend == false && DistanceToPlayer() < (enemy.attackDistance * .7f);

            if (walkAway)
            {
                enemy.SetVelocity((enemy.GetBattleSpeed() * -1) * DirectionToPlayer(), rb.linearVelocity.y);
            }
            else
            {
                enemy.SetVelocity(0, rb.linearVelocity.y);

                if (enemy.PlayerDectection() == false)
                    enemy.HandleFlip(DistanceToPlayer());
            }
        }
    }
}
