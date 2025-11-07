using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void StartGameBtn()
    {
        GameManager.instance.ContinuePlay();
    }

    public void QuitGameBtn()
    {
        Application.Quit();
    }
}
