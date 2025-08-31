using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        var gm = GameManager.instance;
    }

    public void StartGame()
    {
        LevelLoader.Instance.LoadIntro();
    }

    public void OpenSettings()
    {
        GameMenuManager.Instance.OpenSettingsMenu();
    }

    public void QuitGame()
    {
        GameMenuManager.Instance.QuitGame();
    }
}
