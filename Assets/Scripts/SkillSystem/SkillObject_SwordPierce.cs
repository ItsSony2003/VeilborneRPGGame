using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int pierceAmount;

    public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetUpSword(swordManager, direction);
        pierceAmount = swordManager.pierceAmount;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundIsHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (pierceAmount <= 0 || groundIsHit)
        {
            DamageEnemiesInRadius(transform, 0.25f);
            StopSword(collision);
        }

        pierceAmount--;
        DamageEnemiesInRadius(transform, 0.25f);
    }
}
