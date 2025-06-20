using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void CurrentStateTrigger()
    {
        // get asset to player and let current player's state know that we want to exit state
        entity.CurrentStateAnimationTrigger();
    }
}
