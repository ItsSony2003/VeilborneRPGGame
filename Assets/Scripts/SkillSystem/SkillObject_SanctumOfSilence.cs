using UnityEngine;

public class SkillObject_SanctumOfSilence : SkillObject_Base
{
    private Skill_SanctumOfSilence ultimateManager;
    private float expandSpeed = 1.8f;
    private float duration;
    private float slowPercent = 0.95f;

    private Vector3 targetScale;
    private bool isShrinking;

    public void SetUpSanctum(Skill_SanctumOfSilence ultimateManager)
    {
        this.ultimateManager = ultimateManager;

        duration = ultimateManager.GetSanctumDuration();
        slowPercent = ultimateManager.GetSlowPercent();
        expandSpeed = ultimateManager.expandSanctumSpeed;
        float maxSize = ultimateManager.maxSanctumSize;

        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrinkSanctum), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > 0.1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDifference < 0.1f)
            TerminateSanctum();
    }

    private void TerminateSanctum()
    {
        ultimateManager.ClearInsideTargets();
        Destroy(gameObject);
    }

    private void ShrinkSanctum()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        ultimateManager.AddInsideTarget(enemy);
        enemy.SlowDownEntity(duration, slowPercent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.StopSlowDown();
    }
}
