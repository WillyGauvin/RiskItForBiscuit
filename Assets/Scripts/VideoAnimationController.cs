using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoAnimationController : MonoBehaviour
{
    [SerializeField] VideoPlayer video;
    [SerializeField] Animator animator;

    [SerializeField] PlayerInput input;

    bool playingVideo;

    [SerializeField] bool triggerPlay;

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
