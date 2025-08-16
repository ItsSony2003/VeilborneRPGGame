using UnityEngine;

public class SkillObject_EchoOfTheLost : SkillObject_Base
{
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_EchoOfTheLost echoManager;

    public void SetUpEcho(Skill_EchoOfTheLost echoManager)
    {
        this.echoManager = echoManager;

        Invoke(nameof(HandleCloneDeath), echoManager.GetEchoDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
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
