using UnityEngine;

public class UI_Death : MonoBehaviour
{
    public void GoToMarketBtn()
    {
        GameManager.instance.ChangeScene("Market", RespawnType.None);
    }

    public void GoToLastCheckpointBtn()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenuBtn()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
    }
}
