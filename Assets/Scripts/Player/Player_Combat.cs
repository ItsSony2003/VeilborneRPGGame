using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = 0.3f;

    public bool CounterAttackPerform()
    {
        bool hasCountered = false;

        foreach (var target in GetDetectionColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null)
                continue; //skip this target, go to next target

            if(counterable.CanBeCountered)
            {
                counterable.HandleCounterAtack();
                hasCountered = true;
            }
        }
        return hasCountered;
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
