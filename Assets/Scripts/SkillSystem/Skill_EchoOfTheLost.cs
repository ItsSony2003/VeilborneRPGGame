using UnityEngine;

public class Skill_EchoOfTheLost : Skill_Base
{
    [SerializeField] private GameObject echoOfTheLostPrefab;
    [SerializeField] private float echoOfTheLostDuration;

    public float GetEchoDuration()
    {
        return echoOfTheLostDuration;
    }

    public override void TryToUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreateEchoOfTheLost();
    }

    public void CreateEchoOfTheLost()
    {
        GameObject echo = Instantiate(echoOfTheLostPrefab, transform.position, Quaternion.identity);
        
        // Setup echo of the lost
        echo.GetComponent<SkillObject_EchoOfTheLost>().SetUpEcho(this);
    }
}
