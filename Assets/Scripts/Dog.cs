using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class Dog : MonoBehaviour
{
    [Header("Running")]
    [SerializeField] float accelerationRate;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float currentSpeed = 0.0f;

    [Header("Jumping")]
    [SerializeField] float jumpAccelerationRate;
    [SerializeField] float maxJumpForce = 20.0f;
    [SerializeField] float currentJumpForce = 0.0f;

    [Header("Trajectory")]
    [SerializeField] Projection projection;

    [Header("Trainer")]
    [SerializeField] Trainer myTrainer;
    public Trainer MyTrainer => myTrainer;

    [Header("BodyParts")]
    [SerializeField] HingeJoint2D bottomJaw;
    //[SerializeField] CatchDetector detector;

    [Header("Cameras")]
    [SerializeField] CinemachineCamera FollowCam;
    [SerializeField] CinemachineCamera DockCam;

    [Header("Detection")]
    [SerializeField] FrisbeeCatcher frisbeeCatchDetection;
    [SerializeField] JumpAndLandDetection jumpLandDetection;

    [Header("AnimationSettings")]
    [SerializeField] DogAnimationManager animationManager;


    Vector3 jumpForce;
    Vector3 startingPos;
    Quaternion startingRot;


    private Rigidbody2D body;

    [Header("States")]
    public bool isRunning = false;
    public bool isCharging = false;
    public bool hasJumped = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && !hasJumped)
        {
            jumpForce = transform.up * currentJumpForce;
            projection.SimulateTrajectory(transform.position, jumpForce, body.linearVelocity);
        }
    }

    public void Reset()
    {
        StopAllCoroutines();

        GetComponent<LineRenderer>().enabled = false;
        body.linearVelocity = Vector3.zero;
        transform.position = startingPos;
        transform.rotation = startingRot;
        isRunning = false;
        isCharging = false;
        currentJumpForce = 0.0f;
        currentSpeed = 0.0f;
        jumpForce = Vector3.zero;
        hasJumped = false;
        //detector.Reset();
        if (DockCam != null) { DockCam.enabled = true; }
        frisbeeCatchDetection.Reset();
        animationManager.Reset();
    }

    public void BeginRun()
    {
        if (!isRunning)
        {
            StartCoroutine(Run());
            GetComponent<LineRenderer>().enabled = true;
        }
    }

    IEnumerator Run()
    {
        isRunning = true;
        while (true)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, accelerationRate * Time.fixedDeltaTime);

            body.linearVelocity = transform.forward * currentSpeed;

            yield return new WaitForFixedUpdate();
        }
    }

    public void ChargeJump()
    {
        if (!isCharging && isRunning)
        {
            StartCoroutine(ChargeDogJump());
        }
    }

    public void Jump()
    {
        if (!isCharging) return;
        if (hasJumped) return;
        hasJumped = true;
        StopAllCoroutines();

        animationManager.ActivateRigidbody();

        StartCoroutine(RealJump());

        // body.linearVelocity = transform.forward * currentSpeed;
        // body.AddForce(jumpForce, ForceMode2D.Impulse);

        // if (myTrainer != null) { myTrainer.ThrowFrisbee(projection); }
        // if (DockCam != null) { DockCam.enabled = false; }
    }

    IEnumerator RealJump()
    {
        yield return null;
        body.linearVelocity = transform.forward * currentSpeed;
        body.AddForce(jumpForce, ForceMode2D.Impulse);

        // Start tracking score while airborn.
        jumpLandDetection.IncrementScore();

        if (myTrainer != null) { myTrainer.ThrowFrisbee(projection); }
        if (DockCam != null) { DockCam.enabled = false; }
    }

    IEnumerator ChargeDogJump()
    {
        isCharging = true;
        while (true)
        {
            currentJumpForce = Mathf.MoveTowards(currentJumpForce, maxJumpForce, jumpAccelerationRate * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }
    }

    public void OpenMouth()
    {
        //if (!detector.caught)
        {
            JointMotor2D motor = bottomJaw.motor;
            motor.motorSpeed = 100f;
            bottomJaw.motor = motor;
        }
    }

    public void CloseMouth()
    {
        //if (!detector.caught)
        {
            JointMotor2D motor = bottomJaw.motor;
            motor.motorSpeed = -100f;
            bottomJaw.motor = motor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Jump();
    }

    public void SwimAfterDive()
    {
        if (!hasJumped) { return; }

        StartCoroutine(BeginSwim());
    }

    IEnumerator BeginSwim()
    {
        body.linearVelocity = Vector3.zero;
        currentSpeed = 0.0f;
        while (true)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, (accelerationRate / 2.0f) * Time.fixedDeltaTime);

            body.linearVelocity = -transform.forward * currentSpeed;

            yield return new WaitForFixedUpdate();
        }
    }
}
