using UnityEngine;

public class SkillObject_AnimationTrigger : MonoBehaviour
{
    private SkillObject_EchoOfTheLost echoOfTheLost;

    private void Awake()
    {
        echoOfTheLost = GetComponentInParent<SkillObject_EchoOfTheLost>();
    }

    private void AttackTrigger()
    {
        echoOfTheLost.ClonePerformAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if (currentAttackIndex == echoOfTheLost.maxCloneAttacks)
            echoOfTheLost.HandleCloneDeath();
    }
}
