using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    HingeJoint bottomMouth;

    public void OnBite(InputAction.CallbackContext context)
    {
        //If the button is pressed, change the motor to be closing
        if (context.started)
        {
            //Since it is a struct, we have to replace it, we can't just set the target velocity on its own
            JointMotor motor = bottomMouth.motor;
            motor.targetVelocity = 250f;
            bottomMouth.motor = motor;
        }
        //If the button is no longer being pressed, change the motor to be opening
        else if (context.canceled)
        {
            JointMotor motor = bottomMouth.motor;
            motor.targetVelocity = -250f;
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
