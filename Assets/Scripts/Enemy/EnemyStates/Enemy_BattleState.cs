using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform lastTarget;
    protected float lastTimeInBattle;
    protected float lastTimeAttack = float.NegativeInfinity;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UpdateBattleTimer();

        // player ??= enemy.GetPlayerReference();
        if (player == null)
            player = enemy.GetPlayerReference();

        if (ShouldRetreat())
        {
            ShortRetreat();
        }
    }

    protected void ShortRetreat()
    {
        float x = (enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer();
        float y = enemy.retreatVelocity.y;

        rb.linearVelocity = new Vector2(x, y);
        enemy.HandleFlip(DirectionToPlayer());
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDectection())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDectection() && CanAttack())
        {
            lastTimeAttack = Time.time;
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            float xVelocity = enemy.canChasePlayer ? enemy. GetBattleSpeed() : 0.0001f;
            enemy.SetVelocity(xVelocity * DirectionToPlayer(), rb.linearVelocity.y);
        }
    }

    protected bool CanAttack() => Time.time > lastTimeAttack + enemy.attackCooldown;

    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDectection() == false)
            return;

        Transform newTarget = enemy.PlayerDectection().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }

    protected void UpdateBattleTimer() => lastTimeInBattle = Time.time;

    protected bool BattleTimeIsOver() => Time.time > lastTimeInBattle + enemy.battleTimeDuration;

    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    protected bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    protected float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
