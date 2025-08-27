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
            StopAllCoroutines();
            scoreDisplay.SetActive(false);

            ScoreManager.instance.AddToScore((uint)scoreIncrement);
            ScoreManager.instance.TotalPointsForJump();

            ShowDiveStats();
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

    public IEnumerator IncrementScore()
    {
        scoreIncrement = 0.0f;
        scoreText.text = "0";
        scoreDisplay.SetActive(true);

        while (true)
        {
            scoreIncrement += Time.fixedDeltaTime * scoreMultiplier;

            scoreText.text = Mathf.Ceil(scoreIncrement).ToString();

            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator ShowDiveStats()
    {
        // Start Dog swimming left...

        for (int i = 0; i < ScoreManager.instance.NumStatsToDisplay; i++)
        {
            GameObject go = Instantiate(statsDisplayUI, transform);

            var ui = go.GetComponent<FloatingUI>();
            ui.SetupIndicator(transform, thisCollider);
            ui.SetText("Flips: " + ScoreManager.instance.numFlips);

            yield return new WaitForSeconds(1.0f);
        }


        // Create UI
        // Set Text
        // Do for flips, hoops, distance
        // Points earned for this dive
        yield return null;

        // If dives remain after being performed...
        if (DayManager.instance.DivePerformed())
        {
            ScoreManager.instance.ResetDive();
            dog.Reset();
        }
    }
}
