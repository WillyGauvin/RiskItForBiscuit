using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameFrisbeeCatcher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] MinigameFrisbeeDetector topJaw;
    [SerializeField] MinigameFrisbeeDetector bottomJaw;
    [SerializeField] HingeSetter mouthHinge;

    [SerializeField] MinigameFrisbee frisbee;

    [SerializeField] Animator animator;

    PlayerInput input;

    bool caughtFrisbee = false;

    void OnEnable()
    {
        input = GetComponent<PlayerInput>();
        caughtFrisbee = false;
        StartCoroutine(WaitForStart());
        if (mouthHinge)
        {
            mouthHinge.SetSpeed(800f);
        }
    }

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (topJaw.touchingFrisbee && bottomJaw.touchingFrisbee && mouthHinge.GetAngle() < 20f && caughtFrisbee == false)
        {
            caughtFrisbee = true;
            StopAllCoroutines();
            StartCoroutine(WaitForCelebration());
        }
    }

    void StartMinigame()
    {
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap("Minigame");

        StartCoroutine(WaitForSetup());
    }

    IEnumerator WaitForSetup()
    {
        yield return new WaitForSeconds(2f);
        frisbee.SetFrisbee();
        StartCoroutine(WaitForFrisbeeCatch());
    }

    IEnumerator WaitForCelebration()
    {
        yield return new WaitForSeconds(1f);
        CloseGame();
    }

    IEnumerator WaitForStart()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        StartMinigame();
    }

    IEnumerator WaitForFrisbeeCatch()
    {
        yield return new WaitForSeconds(4f);
        CloseGame();
    }

    void CloseGame()
    {
        if (caughtFrisbee)
        {
            Debug.Log("Yay!");
        }
        else
        {
            Debug.Log("Aww! :(");
        }
        float motorSpeed = mouthHinge.GetSpeed();
        input.SwitchCurrentActionMap("Game");
        mouthHinge.SetSpeed(motorSpeed);
        animator.SetTrigger("EndGame");
    }
}
