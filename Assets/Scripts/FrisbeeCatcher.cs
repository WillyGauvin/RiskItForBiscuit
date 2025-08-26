using UnityEngine;

public class FrisbeeCatcher : MonoBehaviour
{
    float timer;
    GameObject frisbee;
    [SerializeField] uint frisbeeCatchScore = 1000;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Frisbee"))
        {
                Debug.Log("Caught");
                frisbee = other.gameObject;
                frisbee.GetComponent<Rigidbody>().isKinematic = true;
                frisbee.GetComponent<Collider>().enabled = false;
                frisbee.transform.parent = this.gameObject.transform;
                frisbee.transform.localPosition = Vector3.zero;

                // You caught the frisbee! Add additional score!
                ScoreManager.instance.AddToScore(frisbeeCatchScore);
        }
    }

    public void Reset()
    {
        if (frisbee != null)
        {
            Destroy(frisbee);
        }
        frisbee = null;
        timer = 0f;
    }
}
