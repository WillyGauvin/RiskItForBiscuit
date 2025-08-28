using Unity.Cinemachine;
using UnityEngine;

public class DockManager : MonoBehaviour
{
    private static DockManager _instance;

    public static DockManager instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                _instance = FindFirstObjectByType<DockManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject[] docks;

    [SerializeField] private Dog player;

    [SerializeField] private int dockSize = 1;
    public int DockSize
    {
        get { return dockSize; }
        set { dockSize = Mathf.Max(value, maxDockSize); }
    }

    private int maxDockSize = 4;

    private void Awake()
    {
    }

    private void Start()
    {
        //Disable renderer of every dock
        foreach(GameObject dock in docks)
        {
            dock.GetComponentInChildren<Renderer>().enabled = false;
        }

        for (int i = 0; i < dockSize; i++)
        {
            docks[i].GetComponentInChildren<Renderer>().enabled = true;
        }

        player.transform.position = docks[dockSize - 1].transform.position;

    }


    /// <summary>
    /// Increases dock size by a factor of multiplier
    /// </summary>
    /// <param name="multiplier"></param>
    public void IncreaseDockSize(int multiplier)
    {
        dockSize *= multiplier;
    }
}
