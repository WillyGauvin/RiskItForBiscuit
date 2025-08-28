using UnityEngine;

public class MinigameFrisbeeDetector : MonoBehaviour
{
    public bool touchingFrisbee { get; private set; }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Frisbee"))
        {
            touchingFrisbee = true;
        }
    }

    void OnEnable()
    {
        touchingFrisbee = false;
    }
}
