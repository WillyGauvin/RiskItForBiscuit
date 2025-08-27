using UnityEngine;

public class JumpAndLandDetection : MonoBehaviour
{
    [SerializeField] Dog dog;

    [Header("Score")]
    [SerializeField] float scoreMultiplier = 10.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            AccumulateScore();

            ScoreManager.instance.TotalPointsForJump();

            ShowDiveStats();

            // If dives remain after being performed...
            if (DayManager.instance.DivePerformed())
            {
                ScoreManager.instance.ResetDive();
                dog.Reset();
            }
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

    public void ShowDiveStats()
    {
        // Create UI
        // Set Text
        // Do for flips, hoops, distance
        // Points earned for this dive
    }
}
