using UnityEngine;

public class FrisbeeCatchDetection : MonoBehaviour
{

    [SerializeField] HingeJoint2D hinge;
    [SerializeField] FrisbeeDetector topDetector;
    [SerializeField] FrisbeeDetector bottomDetector;
    [SerializeField] Rigidbody2D bottomJawRB;
    private GameObject frisbee;

    [SerializeField] uint frisbeeCatchScore = 1000;

    void Update()
    {
        if (frisbee == null)
        {
            if (hinge.jointAngle > 20f && topDetector.touchingFrisbee && bottomDetector.touchingFrisbee)
            {
                Debug.Log("Caught");
                frisbee = topDetector.frisbee;
                frisbee.GetComponent<Rigidbody>().isKinematic = true;
                frisbee.GetComponent<Collider>().enabled = false;
                frisbee.transform.parent = this.gameObject.transform;
                bottomJawRB.freezeRotation = true;

                // You caught the frisbee! Add additional score!
                ScoreManager.instance.AddToScore(frisbeeCatchScore);
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
