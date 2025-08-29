using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class LegControls : MonoBehaviour
{

    private static LegControls _instance;

    public static LegControls instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                _instance = FindFirstObjectByType<LegControls>();
            }
            return _instance;
        }
    }

    [Header("Hinges")]
    [SerializeField] HingeSetter backCalf;
    private bool contractingBackCalf;

    [SerializeField] HingeSetter backThigh;
    private bool contractingBackThigh;

    [SerializeField] HingeSetter frontCalf;
    private bool contractingFrontCalf;

    [SerializeField] HingeSetter frontThigh;
    private bool contractingFrontThigh;

    [SerializeField] HingeJoint2D[] tailPieces;
    private bool waggingTail;

    [Header("Flipping Stats")]

    [Range(.10f, .90f)]
    [SerializeField] float calfFlipInfluence = .25f;
    float thighFlipInfluence;

    [Range(.1f, 1f)]
    [SerializeField] float maxTorqueAmount;

    private bool touchingToes;

    private float torqueFrontFlipAmount;

    private float torqueBackFlipAmount;

    [SerializeField] Rigidbody2D rb2d;

    [Header("Motor Stats")]
    [SerializeField] float onMotorSpeed;

    [SerializeField] HingeJoint2D head;

    [SerializeField] bool canFlip;
    [SerializeField] bool canTouchToes;
    [SerializeField] bool canWagTail;

    private float desiredAngle = 0;
    private bool touchScored;

    void Start()
    {
        thighFlipInfluence = 1 - calfFlipInfluence;
    }

    #region Controls

    public void OnContractBackCalf(InputAction.CallbackContext context)
    {
        if (canFlip || canTouchToes)
        {
            if (context.canceled)
            {
                contractingBackCalf = false;
                backCalf.ResetHinge();
            }
            else
            {
                contractingBackCalf = true;
                backCalf.SetSpeed(onMotorSpeed);
            }
        }
    }

    public void UnlockUpgrade(string id)
    {
        switch (id)
        {
            case "Wag":
                canWagTail = true;
                break;
            case "Flips":
                canFlip = true;
                break;
            case "TouchToes":
                canTouchToes = true;
                break;
                
        }
    }

    public void OnContractBackThigh(InputAction.CallbackContext context)
    {
        if (canFlip || canTouchToes)
        {
            if (context.canceled)
            {
                contractingBackThigh = false;
                backThigh.ResetHinge();
            }
            else
            {
                contractingBackThigh = true;
                backThigh.SetSpeed(onMotorSpeed);
            }
        }

    }

    public void OnContractFrontCalf(InputAction.CallbackContext context)
    {
        if (canFlip || canTouchToes)
        {
            if (context.canceled)
            {
                contractingFrontCalf = false;
                frontCalf.ResetHinge();
            }
            else
            {
                contractingFrontCalf = true;
                frontCalf.SetSpeed(onMotorSpeed);
            }
        }

    }

    public void OnContractFrontThigh(InputAction.CallbackContext context)
    {
        if (canFlip || canTouchToes)
        {
            if (context.canceled)
            {
                contractingFrontThigh = false;
                frontThigh.ResetHinge();
            }
            else
            {
                contractingFrontThigh = true;
                frontThigh.SetSpeed(onMotorSpeed);
            }
        }

    }

    public void OnTailWag(InputAction.CallbackContext context)
    {
        if (canWagTail)
        {
            if (context.canceled)
            {
                waggingTail = false;
                foreach (HingeJoint2D joint in tailPieces)
                {
                    JointMotor2D motor2D = joint.motor;
                    motor2D.motorSpeed = 0f;
                    joint.motor = motor2D;
                    joint.useMotor = false;
                }
            }
            else
            {
                waggingTail = true;
                foreach (HingeJoint2D joint in tailPieces)
                {
                    joint.useMotor = true;
                }
            }
        }

    }

    public void TouchToes()
    {
        if (contractingBackCalf && contractingBackThigh && contractingFrontCalf && contractingFrontThigh)
        {
            Debug.Log("Attempting to touch toes");
            if (touchingToes == false)
            {
                touchingToes = true;
                backCalf.SetSpeed(-onMotorSpeed * 2f);
                backThigh.SetSpeed(-onMotorSpeed * 2f);
                frontCalf.SetSpeed(-onMotorSpeed * 2f);
                frontThigh.SetSpeed(onMotorSpeed * 2f);
            }
        }
        else
        {
            touchingToes = false;
            touchScored = false;
        }
        if (contractingBackCalf && contractingBackThigh && contractingFrontCalf && contractingFrontThigh)
            {
                if (backCalf.AtMin() && backThigh.AtMin() && frontCalf.AtMin() && frontThigh.AtMax() && touchScored == false)
                {
                    Debug.Log("Toes scored");
                    touchScored = true;
                    ScoreManager.instance.AddToScore(500);
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.point_toeTouch);
            }
        }
    }

    #endregion



    void FixedUpdate()
    {
        if (canTouchToes)
            TouchToes();
        if (canWagTail)
            WagTail();

        torqueBackFlipAmount = 0f;
        torqueFrontFlipAmount = 0f;

        if (canFlip)
        {
            if (!touchingToes)
            {
                TorqueBackFlip();
                TorqueFrontFlip();
            }

            rb2d.AddTorque(torqueFrontFlipAmount + torqueBackFlipAmount);
        }

        desiredAngle = torqueFrontFlipAmount + torqueBackFlipAmount;
        //HeadAngle();
    }

    float accumulatedAngle = 0;
    float currentAngle = 0;
    float previousAngle = 0;

    void Update()
    {
        currentAngle = rb2d.rotation;
        accumulatedAngle += currentAngle - previousAngle;
        previousAngle = currentAngle;
        if (accumulatedAngle < -360 && rb2d.angularVelocity < 0f)
        {
            Debug.Log("Front flip");
            ScoreManager.instance.FlipPerformed();
            accumulatedAngle = 0;
        }
        if (accumulatedAngle > 360 && rb2d.angularVelocity > 0f)
        {
            Debug.Log("Back flip");
            ScoreManager.instance.FlipPerformed();
            accumulatedAngle = 0;
        }
    }

    #region Flip Calculations

    private void TorqueBackFlip()
    {
        float percentCompleteCalf = 0f;
        float percentCompleteThigh = 0f;
        if (contractingBackCalf)
        {
            percentCompleteCalf = Mathf.Clamp01((backCalf.GetMaxAngle() - backCalf.GetAngle()) / backCalf.GetMaxAngle());
        }
        if (contractingBackThigh)
        {
            percentCompleteThigh = Mathf.Clamp01((backThigh.GetMaxAngle() - backThigh.GetAngle()) / backThigh.GetMaxAngle());
        }
        torqueBackFlipAmount = (percentCompleteCalf * calfFlipInfluence * maxTorqueAmount) + (percentCompleteThigh * thighFlipInfluence * maxTorqueAmount);
    }

    private void TorqueFrontFlip()
    {
        float percentCompleteCalf = 0f;
        float percentCompleteThigh = 0f;
        if (contractingFrontCalf)
        {
            percentCompleteCalf = Mathf.Clamp01((frontCalf.GetMaxAngle() - frontCalf.GetAngle()) / frontCalf.GetMaxAngle());
        }
        if (contractingFrontThigh)
        {
            percentCompleteThigh = Mathf.Clamp01((frontThigh.GetMaxAngle() - frontThigh.GetAngle()) / frontThigh.GetMaxAngle());
        }
        torqueFrontFlipAmount = -((percentCompleteCalf * calfFlipInfluence * maxTorqueAmount) + (percentCompleteThigh * thighFlipInfluence * maxTorqueAmount));
    }
    #endregion



    #region Extremities Movement

    //Test head movement
    private void HeadAngle()
    {
        if ((desiredAngle - head.jointAngle) > 1f)
        {
            JointMotor2D motor2D = head.motor;
            motor2D.motorSpeed = -30f;
            head.motor = motor2D;
        }
        else if ((desiredAngle - head.jointAngle) < -1f)
        {
            JointMotor2D motor2D = head.motor;
            motor2D.motorSpeed = 30f;
            head.motor = motor2D;
        }
        else
        {
            JointMotor2D motor2D = head.motor;
            motor2D.motorSpeed = 0f;
            head.motor = motor2D;
        }
    }

    private void WagTail()
    {
        if (waggingTail)
        {
            foreach (HingeJoint2D joint in tailPieces)
            {
                if (joint.motor.motorSpeed == 0)
                {
                    JointMotor2D motor2D = joint.motor;
                    motor2D.motorSpeed = 300f;
                    joint.motor = motor2D;
                }

                if (joint.limits.max - joint.jointAngle < 1 || joint.jointAngle - joint.limits.min < 1)
                {
                    JointMotor2D motor2D = joint.motor;
                    motor2D.motorSpeed = -motor2D.motorSpeed;
                    joint.motor = motor2D;
                }
            }
        }
    }
#endregion

}
