using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    private static GameMenuManager thisInstance;

    public static GameMenuManager instance
    {
        get
        {
            if (!thisInstance)
            {
                // Spawns GameManager prefab and sets component reference.
                GameObject manager = Instantiate(Resources.Load<GameObject>("Managers/GameMenuManager"));
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    private void Awake()
    {
        if (thisInstance == null)
        {
            thisInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pauseMenu = Object.FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include).gameObject;
        settingsMenu = Object.FindFirstObjectByType<SettingsMenu>(FindObjectsInactive.Include).gameObject;

        if (pauseMenu == null) Debug.LogError("Pause Menu not found");
        if (settingsMenu == null) Debug.LogError("Settings Menu not found");
    }

    private GameObject pauseMenu;
    private GameObject settingsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetMenus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetMenus()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        ResetMenus();
        pauseMenu.SetActive(true);
        PlayerController.instance.SwitchToActionMap(PlayerController.ActionMap.UI);
        Debug.Log("Paused");
    }

    public void UnPause()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            ResetMenus();
            PlayerController.instance.SwitchToActionMap(PlayerController.ActionMap.Player);
            Debug.Log("Unpaused");
        }
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
