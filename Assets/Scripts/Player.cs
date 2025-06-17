using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public PlayerInputSet input { get; private set; }
    private StateMachine stateMachine;
    
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

    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;

    [Range(0f, 1f)]
    public float wallSlideSlowMultiplier = 0.4f;

    [Range(0f, 1f)]
    public float inAirMoveMultiplier = 0.55f; // should be 0 to 1

    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = -20;
    public Vector2 moveInput { get; private set; }

    [Header("Collision Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform mainWallCheck;
    [SerializeField] private Transform secondWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerInputSet();

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
    }

    private void OnEnable()
    {
        input.Enable();

        // input.Player.Movement.started // Input just begun
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Input in performed, ctx means context
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Input stops, when you release the key
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
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

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        // if moving right and not facing right, flip transform
        // if moving left and  facing right, flip transform
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection *= -1;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(mainWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    // Draw raycast line to detect the ground for Player
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(mainWallCheck.position, mainWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        Gizmos.DrawLine(secondWallCheck.position, secondWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
