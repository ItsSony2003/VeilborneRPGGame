using UnityEngine;

public class SkillObject_EchoOfTheLost : SkillObject_Base
{
    [SerializeField] private float remnantMoveSpeed = 12;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer;

    private Transform playerTransform;
    private Skill_EchoOfTheLost echoManager;
    private TrailRenderer remnantTrail;
    private Entity_Health playerHealth;
    private SkillObject_Health cloneHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler StatusHandler;

    public int maxCloneAttacks {  get; private set; }

    public void SetUpEcho(Skill_EchoOfTheLost echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxCloneAttacks = echoManager.GetMaxCloneAttacks();
        playerTransform = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        skillManager = echoManager.skillManager;
        StatusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleCloneDeath), echoManager.GetCloneDuration());
        CloneFlipToTarget();

        cloneHealth = GetComponent<SkillObject_Health>();
        remnantTrail = GetComponentInChildren<TrailRenderer>();
        remnantTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxCloneAttacks > 0);
    }

    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleRemnantMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }
    }

    private void HandleRemnantTouchPlayer()
    {
        float healAmount = cloneHealth.lastDamageTaken * echoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);

        float amountInSeconds = echoManager.GetCooldownReducedInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);

        if (echoManager.CanRemoveNegativeEffect())
            StatusHandler.REmoveAllNegativeEffects();
    }

    private void HandleRemnantMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, remnantMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < 0.4f)
        {
            HandleRemnantTouchPlayer();
            Destroy(gameObject);
        }
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

        if (echoManager.CanBeRemnant())
            TurnToRemnant();
        else
            Destroy(gameObject);
    }

    private void TurnToRemnant()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        remnantTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (groundHit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
