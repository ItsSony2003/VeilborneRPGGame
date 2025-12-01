using UnityEngine;

public class Enemy_MageCultist : Enemy
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_MageCultistBattleState mageCultistBattleState { get; set; }

    [Header("Mage Specifics")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballStartPoint;
    [SerializeField] private float fireballSpeed = 10;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        mageCultistBattleState = new Enemy_MageCultistBattleState(this, stateMachine, "battle");
        battleState = mageCultistBattleState;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void SpecialAttack()
    {
        GameObject newFireball = Instantiate(fireballPrefab, fireballStartPoint.position, Quaternion.identity);
        newFireball.GetComponent<Enemy_MageFireball>().SetupFireball(fireballSpeed * facingDirection, combat);
    }

    public void HandleCounterAtack()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
}
