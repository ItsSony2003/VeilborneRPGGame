using UnityEngine;

public class Enemy_TwistedRangeProjectile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private LayerMask whatCanCollide;
    //[SerializeField] private LayerMask whatIsTarget;
    //[SerializeField] private LayerMask whatIsGround;

    public void SetupProjectile(Transform target, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        anim.enabled = false;
        this.combat = combat;

        Vector2 velocity = CalculateFireballVelocity(transform.position, target.position);
        rb.linearVelocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if collide object is on a layer we want to damage
        if (((1 << collision.gameObject.layer) & whatCanCollide) != 0)
        {
            // Damage
            combat.PerformAttackOnTarget(collision.transform);

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.enabled = true;
            col.enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }

    private Vector2 CalculateFireballVelocity(Vector2 start, Vector2 end)
    {
        // Get effective gravity based on global gravity and this rb's gravity scale
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        // Calculate vertical and horizontal displacement
        float displacementY = end.y - start.y;
        float displacementX = end.x - start.x;

        float peakHeight = Mathf.Max(arcHeight, end.y - start.y + 0.1f); // Ensure arc is above

        // Time to reach the top of the arc
        float timeToTop = Mathf.Sqrt(2 * peakHeight / gravity);

        // TIme to fall from the top to the target
        float timeFromTop = Mathf.Sqrt(2 * (peakHeight - displacementY) / gravity);

        // Total flight time
        float totalTime = timeToTop + timeFromTop;

        // initial vertical velocity to reach the arc height
        float velocityY = Mathf.Sqrt(2 * gravity * peakHeight);

        // initial horizontal velocity to cover distance in total flight time
        float velocityX = displacementX / totalTime;

        // return combine velocity
        return new Vector2(velocityX, velocityY);
    }
}
