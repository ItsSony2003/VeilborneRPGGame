using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SwordRicochet : SkillObject_Sword
{
    [SerializeField] private float bounceSpeed = 18;
    private int bounceCount;

    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();

    public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        anim.SetTrigger("swordSpin");
        base.SetUpSword(swordManager, direction);

        bounceSpeed = swordManager.bounceSpeed;
        bounceCount = swordManager.bounceCount;
    }

    protected override void Update()
    {
        HandleSwordComeback();
        HandleBounce();
    }

    private void HandleBounce()
    {
        if (nextTarget == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextTarget.position) < 0.8f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceToNextTarget();

            if (bounceCount == 0 || nextTarget == null)
            {
                nextTarget = null;
                BackToPlayer();
            }
        }
    }

    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = EnemiesAround(transform, 10);
            rb.simulated = false;
        }
        DamageEnemiesInRadius(transform, 1);

        if (enemyTargets.Length <= 1 || bounceCount == 0)
            BackToPlayer();
        else
            nextTarget = GetNextTarget();
    }

    private Transform GetNextTarget()
    {
        List<Transform> validTargets = GetValidTargets();

        int randomIndex = Random.Range(0, validTargets.Count);

        Transform nextTarget = validTargets[randomIndex];
        selectedBefore.Add(nextTarget);

        return nextTarget;
    }

    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTarget();

        foreach (var enemy in aliveTargets)
        {
            if (enemy != null && selectedBefore.Contains(enemy.transform) == false)
                validTargets.Add(enemy.transform);
        }

        if (validTargets.Count > 0)
            return validTargets;
        else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }
    }

    private List<Transform> GetAliveTarget()
    {
        List<Transform> aliveTargets = new List<Transform>();

        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
                aliveTargets.Add(enemy.transform);
        }

        return aliveTargets;
    }
}
