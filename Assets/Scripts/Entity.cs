using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1;

    [Header("Collision Detection")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform mainWallCheck;
    [SerializeField] private Transform secondWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    // knockback variables
    private Coroutine knockbackCo;
    private bool isKnocked;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void ReceiveKnockback(Vector2 knockback, float duration)
    {
        if (knockback == null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);
        
        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {

    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(mainWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(mainWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    // Draw raycast line to detect the ground for Player
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(mainWallCheck.position, mainWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        if (secondWallCheck != null)
            Gizmos.DrawLine(secondWallCheck.position, secondWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
