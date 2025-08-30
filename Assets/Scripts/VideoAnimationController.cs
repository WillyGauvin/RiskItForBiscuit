using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoAnimationController : MonoBehaviour
{
    [SerializeField] VideoPlayer video;
    [SerializeField] Animator animator;

    [SerializeField] PlayerInput input;

    [SerializeField] UpgradeDataSO item;

    [NonSerialized] public string id;

    bool playingVideo;

    [SerializeField] bool triggerPlay;

    void Start()
    {
        id = item.abilityID;
    }

    void Update()
    {
        if (triggerPlay)
        {
            OnBuy();
            triggerPlay = false;
        }
    }


    public void OnBuy()
    {
        video.Play();
        animator.SetTrigger("VideoIn");
        input.SwitchCurrentActionMap("Tutorial");
        playingVideo = true;
    }

    public void OnClick()
    {
        if (playingVideo)
        {
            animator.SetTrigger("VideoOut");
            input.SwitchCurrentActionMap("Menu");
            this.gameObject.SetActive(false);
        }
    }
}
