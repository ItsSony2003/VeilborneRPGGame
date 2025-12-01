using UnityEngine;

public class Enemy_MageFireball : MonoBehaviour, ICounterable
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private LayerMask whatIsGround;

    private Collider2D col;
    private Rigidbody2D rb;
    private Entity_Combat combat;
    private Animator anim;

    public bool CanBeCountered => true;

    public void SetupFireball(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        this.combat = combat;
        rb.linearVelocity = new Vector2(xVelocity, 0);

        if (rb.linearVelocity.x < 0)
            transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        bool isTarget = (whatIsTarget & (1 << layer)) != 0;
        bool isGround = (whatIsGround & (1 << layer)) != 0;

        // check if collide object is on a layer we want to damage
        if (isTarget)
        {
            // Damage
            combat.PerformAttackOnTarget(collision.transform);
            HitTarget(collision.transform);
        }
        if (isGround)
        {
            HitTarget(null);
        }
    }

    private void HitTarget(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        anim.enabled = false;

        transform.parent = target;

        Destroy(gameObject, 0.1f);
    }

    public void HandleCounterAtack()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * -1, 0);
        transform.Rotate(0, 180, 0);

        int enemyLayer = LayerMask.NameToLayer("Enemy");

        whatIsTarget |= ( 1 << enemyLayer );
    }
}
