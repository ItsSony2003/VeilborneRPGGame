using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Veil Nexus", fileName = "Item Effect Data - Veil Nexus")]
public class ItemEffect_VeilNexusTeleport : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Market")
        {
            Debug.Log("Can't open Veil Nexus in market");
            return;
        }

        Player player = Player.instance;
        Vector3 teleportPosition = player.transform.position + new Vector3(player.facingDirection * 3.5f, 0 + 1.2f);

        Object_VeilNexus.instance.ActivateTeleport(teleportPosition, player.facingDirection);
    }
}
