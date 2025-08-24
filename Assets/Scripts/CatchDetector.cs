using UnityEngine;

public class CatchDetector : MonoBehaviour
{
    [SerializeField]
    HingeJoint bottomJaw;

    private Collider frisbee;
    public bool caught = false;

    float timer = 0.5f;
    void OnTriggerStay(Collider other)
    {
        //If the mouth is closing
        if (bottomJaw.motor.targetVelocity > 0f)
        {
            Debug.Log("Trigger Stay");
            if (other.CompareTag("Frisbee"))
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    CatchFrisbee(other);
                }
            }
        }
        if (bottomJaw.angle > 28f)
        {
            if (other.CompareTag("Frisbee"))
            {
                    CatchFrisbee(other);
            }
        }
    }

    private void CatchFrisbee(Collider other)
    {
        Debug.Log("Caught");
        other.enabled = false;
        frisbee = other;
        frisbee.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        frisbee.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        frisbee.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        frisbee.gameObject.transform.parent = this.gameObject.transform.parent;
        JointMotor motor = bottomJaw.motor;
        motor.targetVelocity = 0f;
        bottomJaw.motor = motor;
        caught = true;
    }

    public void Reset()
    {
        if (frisbee != null)
        {
            Destroy(frisbee.gameObject);
            frisbee = null;
        }
        timer = 0.5f;
        caught = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
            Debug.Log("Trigger Enter");
        }
    }
}
