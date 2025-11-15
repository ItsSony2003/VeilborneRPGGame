using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{

    public void Start()
    {
        transform.root.GetComponentInChildren<UI_Options>(true).LoadVolume();
        transform.root.GetComponentInChildren<UI_ScreenEffect>().FadeIn();

        AudioManager.instance.StartBGM("playlist_menu");
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
