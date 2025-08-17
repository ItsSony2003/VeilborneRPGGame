using UnityEngine;

public class SkillObject_EchoOfTheLost : SkillObject_Base
{
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_EchoOfTheLost echoManager;

    public int maxCloneAttacks {  get; private set; }

    public void SetUpEcho(Skill_EchoOfTheLost echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxCloneAttacks = echoManager.GetMaxCloneAttacks();

        CloneFlipToTarget();
        anim.SetBool("canAttack", maxCloneAttacks > 0);
        Invoke(nameof(HandleCloneDeath), echoManager.GetCloneDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }

    private void CloneFlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void ClonePerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetCloneDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 2f : -2f;

        if (canDuplicate)
            echoManager.CreateClone(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleCloneDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (groundHit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
