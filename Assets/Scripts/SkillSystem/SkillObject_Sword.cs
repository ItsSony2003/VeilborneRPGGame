using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;

    protected Transform playerTransform;
    protected bool shouldComeBack;
    protected float comeBackSpeed = 25;
    protected float maxAllowedDistance = 30;

    protected virtual void Update()
    {
        transform.right = rb.linearVelocity;
        HandleSwordComeback();
    }

    public void BackToPlayer() => shouldComeBack = true;

    public virtual void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        rb.linearVelocity = direction;

        this.swordManager = swordManager;

        playerTransform = swordManager.transform.root;
        playerStats = swordManager.player.stats;
        damageScaleData = swordManager.damageScaleData;
    }

    protected void HandleSwordComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxAllowedDistance)
            BackToPlayer();

        if (shouldComeBack == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comeBackSpeed * Time.deltaTime);

        if (distance < 0.5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform, 1);
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
