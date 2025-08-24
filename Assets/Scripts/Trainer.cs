using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Trainer : MonoBehaviour
{
    [SerializeField] GameObject FrisbeePrefab;
    [Range(0, 1)]
    [SerializeField] float percentageOfArcToThrowTo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowFrisbee(Projection projection)
    {
        Tuple<Vector3, float> pointAndTime = projection.GetPoint(percentageOfArcToThrowTo);

        Vector3 r = pointAndTime.Item1 - transform.position;

        Vector3 launchVelocity = (r - 0.5f * Physics.gravity * pointAndTime.Item2 * pointAndTime.Item2) / pointAndTime.Item2;

        GameObject Frisbee = Instantiate(FrisbeePrefab, transform.position, Quaternion.identity);

        Frisbee.GetComponent<Rigidbody>().AddForce(launchVelocity, ForceMode.Impulse);
    }
}
