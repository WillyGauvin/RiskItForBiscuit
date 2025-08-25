using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Game Manager in the scene. Duplicate deleted.");
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
