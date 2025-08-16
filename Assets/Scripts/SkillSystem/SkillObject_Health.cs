using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    protected override void Die()
    {
        SkillObject_EchoOfTheLost echoOfTheLost = GetComponent<SkillObject_EchoOfTheLost>();
        echoOfTheLost.HandleCloneDeath();
    }
}
