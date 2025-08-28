using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrisbeeCatcherAndDetector : MonoBehaviour
{
    GameObject frisbee;

    [SerializeField] GameObject miniGame;

    List<Rigidbody2D> bodies;
    List<Vector2> velocities = new List<Vector2>();
    List<float> angularVelocities = new List<float>();
    bool interactedWithFrisbee = false;

    void Start()
    {
        bodies = transform.parent.GetComponentsInChildren<Rigidbody2D>().ToList<Rigidbody2D>();
    }



    void OnTriggerStay(Collider other)
    {
        if (interactedWithFrisbee == false)
        {
            if (other.CompareTag("Frisbee"))
            {
                frisbee = other.gameObject;
                interactedWithFrisbee = true;
                StartCoroutine(SlowDownTime());
            }
        }
    }

    public void CaughtFrisbee(bool caughtFrisbee)
    {
        if (caughtFrisbee)
        {
            frisbee.GetComponent<Rigidbody>().isKinematic = true;
            frisbee.GetComponent<Collider>().enabled = false;
            frisbee.transform.parent = this.gameObject.transform;
            frisbee.transform.localPosition = Vector3.zero;
            // You caught the frisbee! No point penalty!
            ScoreManager.instance.SetFrisbeeCaught();
        }

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].bodyType = RigidbodyType2D.Dynamic;
            bodies[i].linearVelocity = velocities[i];
            bodies[i].angularVelocity = angularVelocities[i];
        }
        velocities.Clear();
        angularVelocities.Clear();
    }

    IEnumerator SlowDownTime()
    {
        float timer = 0f;
        while (Time.timeScale > 0f)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1f, 0f, timer);
            yield return null;
        }
        foreach (Rigidbody2D body in bodies)
        {
            velocities.Add(body.linearVelocity);
            angularVelocities.Add(body.angularVelocity);
            body.bodyType = RigidbodyType2D.Static;
        }
        Time.timeScale = 1f;
        miniGame.SetActive(true);
    }

    public void Reset()
    {
        if (frisbee != null)
        {
            Destroy(frisbee);
        }
        frisbee = null;
        interactedWithFrisbee = false;
    }
}
