using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] List<VideoAnimationController> videos;

    private static VideoManager _instance;

    public static VideoManager instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                _instance = FindFirstObjectByType<VideoManager>();
            }
            return _instance;
        }
    }

    public void OnBuy(string id)
    {
        foreach (VideoAnimationController video in videos)
        {
            if (video.id == id)
            {
                video.OnBuy();
            }
        }
    }

    void Start()
    {
        videos = GetComponentsInChildren<VideoAnimationController>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
