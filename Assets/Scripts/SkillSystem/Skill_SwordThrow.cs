using Unity.Hierarchy;
using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;
    private float currentThrowPower;

    [Header("Default Sword Upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0, 10)]
    [SerializeField] private float normalThrowPower = 5;

    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject swordPiercePrefab;
    public int pierceAmount = 5;
    [Range(0, 10)]
    [SerializeField] private float pierceThrowPower = 5;

    [Header("Spin Sword Upgrade")]
    [SerializeField] private GameObject swordSpinPrefab;
    public int maxDistance = 6;
    public float attackPerSecond = 3;
    public float maxSpinDuration = 2.5f;
    [Range(0, 10)]
    [SerializeField] private float spinThrowPower = 5;

    [Header("Ricochet Sword Upgrade")]
    [SerializeField] private GameObject swordRicochetPrefab;
    public int bounceCount = 6;
    public float bounceSpeed = 18;
    [Range(0, 10)]
    [SerializeField] private float ricochetThrowPower = 5;

    [Header("Trajectory Aim")]
    [SerializeField] private GameObject perdictionDot;
    [SerializeField] private int numberOfDots = 15;
    [SerializeField] private float spaceBetweenDots = 0.08f;
    private float swordGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();

        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();

        if (currentSword != null)
        {
            currentSword.BackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void SwordThrow()
    {
        GameObject swordPrefab = GetSwordTypePrefab();
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetUpSword(this, GetThrowDirection());

        SetSkillOnCooldown();
    }

    private GameObject GetSwordTypePrefab()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return swordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return swordPiercePrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return swordSpinPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Ricochet))
            return swordRicochetPrefab;

        Debug.Log("Invalid Sword Upgrade Selected!");
        return null;
    }

    private void UpdateThrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.SwordThrow:
                currentThrowPower = normalThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Spin:
                currentThrowPower = spinThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Ricochet:
                currentThrowPower = ricochetThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Pierce:
                currentThrowPower = pierceThrowPower;
                break;
        }
    }

    private Vector2 GetThrowDirection() => confirmedDirection * (currentThrowPower * 10);

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentThrowPower * 10;

        // This gives us the initial velocity - the starting speed and direciton of the throw
        Vector2 initialVelocity = direction * scaledThrowPower;

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPositon = transform.root.position;

        return playerPositon + predictedPoint;
    }

    public void ConfirmedTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
        {
            t.gameObject.SetActive(enable);
        }
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(perdictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
