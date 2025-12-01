using UnityEngine;

public class Enemy_TwistedRetreatState : EnemyState
{
    private Enemy_TwistedCultist twistedCultist;
    private Vector3 startPosition;
    private Transform player;

    public Enemy_TwistedRetreatState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        twistedCultist = enemy as Enemy_TwistedCultist;
    }

    public override void Enter()
    {
        base.Enter();

        if (player == null)
            player = enemy.GetPlayerReference();

        startPosition = enemy.transform.position;

        rb.linearVelocity = new Vector2(twistedCultist.retreatSpeed * -DirectionToPlayer(), 0);
        enemy.HandleFlip(DirectionToPlayer());

        enemy.gameObject.layer = LayerMask.NameToLayer("Untouchable");
        enemy.vfx.DoDashImageEchoEffect(0.8f);
    }

    public override void Update()
    {
        base.Update();

        bool reachedMaxDistance = Vector2.Distance(enemy.transform.position, startPosition) > twistedCultist.retreatMaxDistance;
        
        if (reachedMaxDistance || twistedCultist.CannotMoveBackward())
            stateMachine.ChangeState(twistedCultist.twistedRangeState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.vfx.StopImageEchoEffect();
        enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
