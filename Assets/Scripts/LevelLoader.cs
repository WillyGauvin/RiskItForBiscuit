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
        StartCoroutine(LoadLevel(1));
    }

    public void LoadNextLevel()
    {
        if (GameManager.instance.IsGameEnded)
        {
            EndGame();
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Dock
            StartCoroutine(LoadLevel(2));
        }
        else
        {
            // Shop
            StartCoroutine(ShowStatsAndLoad(1));
        }
    }

    public void EndGame()
    {
        if (GameManager.instance.IsBadEnding)
        {
            // Game Over - Bad Ending
            StartCoroutine(LoadLevel(3));
        }
        else
        {
            // Victory Screen - Good Ending
            StartCoroutine(LoadLevel(4));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load Scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator ShowStatsAndLoad(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        statsScreen.SetActive(true);

        //Wait
        yield return new WaitForSeconds(transitionTime * 2.0f);

        //Load Scene
        SceneManager.LoadScene(levelIndex);
        ScoreManager.instance.ResetScore();
    }
}
