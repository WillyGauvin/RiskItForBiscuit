using UnityEngine;

public class WaterDetection : MonoBehaviour
{

    [SerializeField] Dog dog;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            dog.AccumulateScore();

            // If dives remain after being performed...
            if (DayManager.instance.DivePerformed())
            {
                dog.Reset();
            }
        }
        if (other.CompareTag("EdgeOfDock"))
        {
            dog.Jump();
        }
    }
}
