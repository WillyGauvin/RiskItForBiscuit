using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [SerializeField] GameObject statsScreen;

    public void Awake()
    {
        if (Instance != null) Debug.LogError("More than one level loader found");
        Instance = this;
    }

    static bool firstTimeJumping = true;
    public static bool tutorialActive = false;

    public Animator transition;
    public float transitionTime = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        statsScreen.SetActive(false);
    }

    public void LoadBeginning()
    {
        // Load Dock.
        StartCoroutine(LoadLevel("Shop"));
    }

    public void LoadNextLevel()
    {
        if (GameManager.instance.IsGameEnded)
        {
            EndGame();
            return;
        }

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            // Dock
            if (firstTimeJumping)
            {
                Debug.Log("First Jump");
                tutorialActive = true;
                firstTimeJumping = false;
            }
            else if (BoughItemStart.boughItemDone == false && ObstacleManager.buildTutorialNeeded)
            {
                BoughItemStart.boughItemDone = true;
            }
            StartCoroutine(LoadLevel("Dock"));
        }
        else if (SceneManager.GetActiveScene().name == "Dock")
        {
            // Shop
            StartCoroutine(ShowStatsAndLoad("Shop"));
        }
    }

    public void EndGame()
    {
        if (GameManager.instance.IsBadEnding)
        {
            // Game Over - Bad Ending
            StartCoroutine(LoadLevel("GameOver"));
        }
        else
        {
            // Victory Screen - Good Ending
            StartCoroutine(LoadLevel("GameVictory"));
        }
    }

    IEnumerator LoadLevel(string levelName)
    {
        //Play animation
        transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load Scene

        SceneManager.LoadScene(levelName);
    }

    IEnumerator ShowStatsAndLoad(string levelName)
    {
        //Play animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        statsScreen.SetActive(true);

        //Wait
        yield return new WaitForSeconds(transitionTime * 2.0f);

        //Load Scene
        SceneManager.LoadScene(levelName);
        ScoreManager.instance.ResetScore();
    }
}
