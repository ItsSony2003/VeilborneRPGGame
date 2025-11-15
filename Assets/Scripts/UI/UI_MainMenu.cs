using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void Start()
    {
        transform.root.GetComponentInChildren<UI_ScreenEffect>().FadeIn();
    }

    public void StartGameBtn()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.ContinuePlay();
    }

    public void QuitGameBtn()
    {
        Application.Quit();
    }
}
