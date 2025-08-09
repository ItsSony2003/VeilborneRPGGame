using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    private UI ui;

    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }

    #region State Variables
    public Player_IdleState idleState { get; private set; }
    public Player_RunState runState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    //public Player_LandState landState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }

    #endregion

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1.5f;
    private Coroutine queueBasicAttackCo;

    [Header("Movement Details")]
    public float runSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    //public float landSpeed = -10;
    [Range(0f, 1f)]
    public float wallSlideSlowMultiplier = 0.4f;
    [Range(0f, 1f)]
    public float inAirMoveMultiplier = 0.55f; // should be 0 to 1
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = -20;

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        input = new PlayerInputSet();
        skillManager = GetComponent<Player_SkillManager>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        runState = new Player_RunState(this, stateMachine, "run");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        //landState = new Player_LandState(this, stateMachine, "land");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalRunSpeed = runSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = attackVelocity;

        float speedMultiplier = 1 - slowMultiplier;
        
        runSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        runSpeed = originalRunSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }
    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    private void OnEnable()
    {
        input.Enable();

        // input.Player.Movement.started // Input just begun
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Input in performed, ctx means context
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Input stops, when you release the key

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queueBasicAttackCo != null)
            StopCoroutine(queueBasicAttackCo);

        queueBasicAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}
