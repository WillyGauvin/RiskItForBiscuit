using UnityEngine;

public class FrisbeeCatchDetection : MonoBehaviour
{

    [SerializeField] HingeJoint hinge;
    [SerializeField] FrisbeeDetector topDetector;
    [SerializeField] FrisbeeDetector bottomDetector;
    [SerializeField] Rigidbody bottomJawRB;
    private GameObject frisbee;

    void Update()
    {
        if (frisbee == null)
        {
            if (hinge.angle > 20f && topDetector.touchingFrisbee && bottomDetector.touchingFrisbee)
            {
                Debug.Log("Caught");
                frisbee = topDetector.frisbee;
                frisbee.GetComponent<Rigidbody>().isKinematic = true;
                frisbee.GetComponent<Collider>().enabled = false;
                frisbee.transform.parent = this.gameObject.transform;
                bottomJawRB.freezeRotation = true;
            }
        }
    }

    public void Reset()
    {
        if (frisbee != null)
        {
            Destroy(frisbee);
        }
        frisbee = null;
        bottomJawRB.freezeRotation = false;
        topDetector.touchingFrisbee = false;
        bottomDetector.touchingFrisbee = false;
    }
}
