using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D collider2D;

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        collider2D = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        base.Enter();

        rb.simulated = false;
        stateMachine.TurnOffStateMachine();

        //THIS IS FOR ENEMY TO FALL OFF MAP
        //anim.enabled = false;
        //collider2D.enabled = false;
        //rb.gravityScale = 12;
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, 12);
        //stateMachine.TurnOffStateMachine();

        // Delete Enemy (can be used later)
        //Object.Destroy(enemy.gameObject, 5f);
    }
}
