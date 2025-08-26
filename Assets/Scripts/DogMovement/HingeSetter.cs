using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class HingeSetter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    HingeJoint2D hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    public void SetSpeed(float speed)
    {
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = speed;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    public float GetAngle()
    {
        return hinge.jointAngle;
    }

    public float GetMaxAngle()
    {
        return hinge.limits.max;
    }

    public float GetMinAngle()
    {
        return hinge.limits.min;
    }

    public bool AtMin()
    {
        return (GetAngle() - GetMinAngle()) < 1;
    }

    public bool AtMax()
    {
        return (GetMaxAngle() - GetAngle()) < 1;
    }

    public void ResetHinge()
    {
        hinge.useMotor = false;
        // JointMotor2D motor = hinge.motor;
        // motor.motorSpeed = speed *2f;
        // hinge.motor = motor;
    }

    private void Update()
    {

        // if (resetHinge)
        // {
        //     if (hinge.jointAngle <= 1f)
        //     {
        //         JointMotor2D motor = hinge.motor;
        //         motor.motorSpeed = 0;
        //         hinge.motor = motor;
        //         resetHinge = false;
        //     }
        // }

    }
}
