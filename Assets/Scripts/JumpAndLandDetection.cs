using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAndLandDetection : MonoBehaviour
{
    [SerializeField] Dog dog;

    [Header("Score")]
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] float scoreIncrement = 0.0f;
    [SerializeField] float scoreMultiplier = 50.0f;

    [SerializeField] GameObject statsDisplayUI;
    Collider thisCollider;
    const float timeToDisplay = 1.5f;

    bool hasLanded = false;

    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            StartCoroutine(ShowDiveStats());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            // Do not allow for a double collision with the water.
            if (hasLanded) { return; }

            hasLanded = true;
            StopAllCoroutines();
            scoreDisplay.SetActive(false);

            ScoreManager.instance.AddToScore((uint)scoreIncrement);
            ScoreManager.instance.TotalPointsForJump();

            StartCoroutine(ShowDiveStats());
        }

        if (other.CompareTag("EdgeOfDock"))
        {
            dog.Jump();
        }
    }

    /// <summary>
    /// Add score based on distance travelled and a score multiplier.
    /// </summary>
    public void AccumulateScore()
    {
        if (!dog.hasJumped || dog.MyTrainer == null) { return; }

        float distanceTravelled = Vector3.Distance(transform.position, dog.MyTrainer.transform.position);
        distanceTravelled *= scoreMultiplier;

        ScoreManager.instance.AddToScore((uint)distanceTravelled);
    }

    public void IncrementScore()
    {
        scoreIncrement = 0.0f;
        scoreText.text = "0";
        scoreDisplay.SetActive(true);

        StartCoroutine(BeginIncrementScore());
    }

    IEnumerator BeginIncrementScore()
    {
        while (true)
        {
            scoreIncrement += Time.fixedDeltaTime * scoreMultiplier;

            scoreText.text = Mathf.Ceil(scoreIncrement).ToString();

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ShowDiveStats()
    {
        // Start Dog swimming left...
        dog.SwimAfterDive();

        for (int i = 0; i < (int)ScoreStats.Max; i++)
        {
            switch (i)
            {
                case (int)ScoreStats.Flip:
                    if (ScoreManager.instance.numFlips > 0)
                    {
                        StartCoroutine(DisplayStat("Flips: ", ScoreManager.instance.numFlips));
                        yield return new WaitForSeconds(timeToDisplay);
                    }
                    break;

                case (int)ScoreStats.Hoop:
                    if (ScoreManager.instance.numHoops > 0)
                    {
                        StartCoroutine(DisplayStat("Hoops: ", ScoreManager.instance.numHoops));
                        yield return new WaitForSeconds(timeToDisplay);
                    }
                    break;

                case (int)ScoreStats.Frisbee:
                    if (ScoreManager.instance.wasFrisbeeCaught)
                    {
                        StartCoroutine(DisplayStat("Frisbee Caught!"));
                    }
                    else
                    {
                        StartCoroutine(DisplayStat("Frisbee Not Caught! x0.5 Points Earned!"));
                    }
                    yield return new WaitForSeconds(timeToDisplay);
                    break;

                case (int)ScoreStats.EarnedScore:
                default:
                    StartCoroutine(DisplayStat("Total Earned Score: ", ScoreManager.instance.earnedScoreForJump));
                    yield return new WaitForSeconds(timeToDisplay);
                    break;
            }
        }

        yield return null;

        // If dives remain after being performed...
        if (DayManager.instance.DivePerformed())
        {
            hasLanded = false;
            ScoreManager.instance.ResetDive();
            dog.Reset();
        }
    }

    IEnumerator DisplayStat(string statName, int numberToDisplay = 0)
    {
        GameObject go = Instantiate(statsDisplayUI, transform.position, Quaternion.identity);

        var ui = go.GetComponent<FloatingUI>();
        ui.SetupIndicator(transform.position, thisCollider);

        if (numberToDisplay > 0)
        {
            ui.SetText(statName + numberToDisplay);
        }
        else
        {
            ui.SetText(statName);
        }

        yield return null;
    }
}
