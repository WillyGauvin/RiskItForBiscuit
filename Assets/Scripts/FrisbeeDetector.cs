using UnityEngine;

public class FrisbeeDetector : MonoBehaviour
{
    public bool touchingFrisbee;

    public GameObject frisbee;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Frisbee"))
        {
            touchingFrisbee = true;
            frisbee = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Frisbee"))
        {
            touchingFrisbee = false;
            frisbee = collision.gameObject;
        }
    }
}
