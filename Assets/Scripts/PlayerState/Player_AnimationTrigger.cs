using UnityEngine;

public class Player_AnimationTrigger : MonoBehaviour
{
    private Player player;
    
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void CurrentStateTrigger()
    {
        // get asset to player and let current player's state know that we want to exit state
        player.CallAnimationTrigger();
    }
}
