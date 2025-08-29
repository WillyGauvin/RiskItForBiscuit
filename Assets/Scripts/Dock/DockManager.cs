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

    [SerializeField] CinemachineFollow dockCam;
    public int DockSize
    {
        get { return dockSize; }
        set { dockSize = Mathf.Max(value, maxDockSize); }
    }

    private int maxDockSize = 3;

    private void Awake()
    {
    }

    private void Start()
    {
        //Disable renderer of every dock
        foreach(GameObject dock in docks)
        {
            Renderer[] renderers = dock.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        for (int i = 0; i < dockSize; i++)
        {
            Renderer[] renderers = docks[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }

        player.transform.position = docks[dockSize - 1].transform.position;


        switch (dockSize)
        {
            case 1:
                dockCam.FollowOffset.z = -50.0f;
                break;
            case 2:
                dockCam.FollowOffset.z = -90.0f;
                break;
            case 3:
                dockCam.FollowOffset.z = -130.0f;
                break;
        }
    }


    /// <summary>
    /// Increases dock size by a factor of multiplier
    /// </summary>
    /// <param name="multiplier"></param>
    public void IncreaseDockSize()
    {
        dockSize += 1;
    }
}
