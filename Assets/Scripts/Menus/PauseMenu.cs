using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnResumeClicked()
    {
        GameMenuManager.instance.UnPause();
    }

    public void OnQuitClicked()
    {
        GameMenuManager.instance.UnPause();
        SceneManager.LoadScene("MainMenu");
    }
}
