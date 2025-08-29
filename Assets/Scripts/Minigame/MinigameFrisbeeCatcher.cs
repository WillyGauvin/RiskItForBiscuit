using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] MinigameFrisbeeDetector topJaw;
    [SerializeField] MinigameFrisbeeDetector bottomJaw;
    [SerializeField] HingeSetter mouthHinge;
    [SerializeField] FrisbeeCatcherAndDetector catcher;

    List<Collider2D> allColliders = new List<Collider2D>();

    [SerializeField] MinigameFrisbee frisbee;

    [SerializeField] MiniDog dog;

    [SerializeField] Animator animator;

    PlayerInput input;

    bool caughtFrisbee = false;

    void OnEnable()
    {
        allColliders = GetComponentsInChildren<Collider2D>().ToList();
        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = false;
        }
        input = GetComponent<PlayerInput>();
        caughtFrisbee = false;

        StartCoroutine(WaitForStart());
        if (mouthHinge)
        {
            mouthHinge.SetSpeed(80f);
        }
    }

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (topJaw.touchingFrisbee && bottomJaw.touchingFrisbee && mouthHinge.GetSpeed() < 0f && caughtFrisbee == false)
        {
            caughtFrisbee = true;
            StopAllCoroutines();
            dog.CantBite();
            StartCoroutine(WaitForCelebration());
        }
    }

    void StartMinigame()
    {
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap("Minigame");

        foreach (Collider2D collider in allColliders)
        {
            collider.enabled = true;
        }

        StartCoroutine(WaitForSetup());
    }

    IEnumerator WaitForSetup()
    {
        yield return new WaitForSeconds(.5f);
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
        StartCoroutine(WaitForEndOfClose());
    }

    IEnumerator WaitForEndOfClose()
    {
        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        catcher.CaughtFrisbee(caughtFrisbee);
        this.gameObject.SetActive(false);
    }
}
