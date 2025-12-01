using UnityEngine;

public class UI_Victory : MonoBehaviour
{
    public void GoToMainMenuVictoryBtn()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }
}
