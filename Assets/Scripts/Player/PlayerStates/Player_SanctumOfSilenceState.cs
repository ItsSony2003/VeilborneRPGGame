using UnityEngine;

public class Player_SanctumOfSilenceState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float maxJumpDistance;

    private bool isFloating;
    private bool createSanctum;

    public Player_SanctumOfSilenceState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        maxJumpDistance = GetAvailableRiseDistance();

        player.SetVelocity(0, player.jumpSkillSpeed);
        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(originalPosition, player.transform.position) >= maxJumpDistance && isFloating == false)
            Floating();

        if (isFloating)
        {
            // skill manager to cast spells
            skillManager.sanctumOfSilence.DoSpellCasting();

            if (stateTimer < 0)
            {
                isFloating = false;
                rb.gravityScale= originalGravity;

                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        createSanctum = false;
        player.health.SetCanTakeDamage(true);
    }

    private void Floating()
    {
        isFloating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        // How long player will stay in the air
        stateTimer = skillManager.sanctumOfSilence.GetSanctumDuration();

        if (createSanctum == false)
        {
            createSanctum = true;

            // skill manager to create skill object
            skillManager.sanctumOfSilence.CreateSanctum();
        }
    }

    private float GetAvailableRiseDistance()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(player.transform.position, Vector2.up, player.jumpSkillMaxDistance, player.whatIsGround);

        return hit.collider != null ? hit.distance - 2 : player.jumpSkillMaxDistance;
    }
}
