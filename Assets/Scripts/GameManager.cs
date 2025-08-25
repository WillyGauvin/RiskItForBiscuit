using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public static GameManager Get()
    //{
    //    if (!instance)
    //    {
    //        // Spawns GameManager prefab and sets component reference.
    //        GameObject manager = Instantiate(Resources.Load<GameObject>("Managers/GameManager"));
    //    }

    //    return instance;
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
