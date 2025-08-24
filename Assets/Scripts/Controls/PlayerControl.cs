using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    HingeJoint bottomMouth;

    public void OnBite(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            JointMotor motor = bottomMouth.motor;
            motor.targetVelocity = -100f;
            bottomMouth.motor = motor;
        }
        else if (context.started)
        {
            JointMotor motor = bottomMouth.motor;
            motor.targetVelocity = 100f;
            bottomMouth.motor = motor;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
