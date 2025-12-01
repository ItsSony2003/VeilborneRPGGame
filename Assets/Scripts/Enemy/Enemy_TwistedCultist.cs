using System.Collections;
using UnityEngine;

public class Enemy_TwistedCultist : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_TwistedRetreatState twistedRetreatState { get; private set; }
    public Enemy_TwistedBattleState twistedBattleState { get; private set; }
    public Enemy_TwistedRangeState twistedRangeState { get; private set; }

    [Header("Twisted Specifics")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballStartPosition;
    [SerializeField] private int amountToCast = 8;
    [SerializeField] private float rangeCastCooldown = 0.2f;
    public bool rangeCastPerform {  get; private set; }
    [Space]
    public float retreatCooldown = 5;
    public float retreatMaxDistance = 10;
    public float retreatSpeed = 20;
    [SerializeField] private Transform behindCollisionCheck;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        twistedRetreatState = new Enemy_TwistedRetreatState(this, stateMachine, "battle");
        twistedBattleState = new Enemy_TwistedBattleState(this, stateMachine, "battle");
        twistedRangeState = new Enemy_TwistedRangeState(this, stateMachine, "rangeCast");

        battleState = twistedBattleState;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void SetRangeCastPerformed(bool performed) => rangeCastPerform = performed;

    public override void SpecialAttack()
    {
        StartCoroutine(RangeCastCo());
    }

    private IEnumerator RangeCastCo()
    {
        for (int i = 0; i < amountToCast; i++)
        {
            Enemy_TwistedRangeProjectile projectile =
                Instantiate(fireballPrefab, fireballStartPosition.position, Quaternion.identity)
                .GetComponent<Enemy_TwistedRangeProjectile>();

            projectile.SetupProjectile(player.transform, combat);
            yield return new WaitForSeconds(rangeCastCooldown);
        }

        SetRangeCastPerformed(true);
    }

    public void HandleCounterAtack()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public bool CannotMoveBackward()
    {
        bool detectedWall = Physics2D.Raycast(behindCollisionCheck.position, Vector2.right * -facingDirection, 1.5f, whatIsGround);
        bool noGround = Physics2D.Raycast(behindCollisionCheck.position, Vector2.down, 1.5f, whatIsGround) == false;

        return noGround || detectedWall;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(behindCollisionCheck.position,
            new Vector3(behindCollisionCheck.position.x + (1.5f * -facingDirection), behindCollisionCheck.position.y));
        
        Gizmos.DrawLine(behindCollisionCheck.position,
            new Vector3(behindCollisionCheck.position.x, behindCollisionCheck.position.y - 1.5f));
    }
}
