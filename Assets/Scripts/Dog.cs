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


    [Header("BodyParts")]
    [SerializeField] HingeJoint bottomJaw;
    //[SerializeField] CatchDetector detector;

    [Header("Cameras")]
    [SerializeField] CinemachineCamera FollowCam;
    [SerializeField] CinemachineCamera DockCam;

    [Header("Detection")]
    [SerializeField] FrisbeeCatchDetection frisbeeCatchDetection;

    [Header("Score")]
    [SerializeField] float scoreMultiplier = 10.0f;




    Vector3 jumpForce;
    Vector3 startingPos;


    private Rigidbody body;

    [Header("States")]
    public bool isRunning = false;
    public bool isCharging = false;
    public bool hasJumped = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
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
        GetComponent<LineRenderer>().enabled = false;
        body.linearVelocity = Vector3.zero;
        transform.position = startingPos;
        StopAllCoroutines();
        isRunning = false;
        isCharging = false;
        currentJumpForce = 0.0f;
        currentSpeed = 0.0f;
        jumpForce = Vector3.zero;
        hasJumped = false;
        //detector.Reset();
        if (DockCam != null) { DockCam.enabled = true; }
        frisbeeCatchDetection.Reset();
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
        if (!isCharging)
        {
            StartCoroutine(ChargeDogJump());
        }
    }

    public void Jump()
    {
        if (hasJumped) return;
        hasJumped = true;
        StopAllCoroutines();

        body.linearVelocity = transform.forward * currentSpeed;
        body.AddForce(jumpForce, ForceMode.Impulse);

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
            JointMotor motor = bottomJaw.motor;
            motor.targetVelocity = -100f;
            bottomJaw.motor = motor;
        }
    }

    public void CloseMouth()
    {
        //if (!detector.caught)
        {
            JointMotor motor = bottomJaw.motor;
            motor.targetVelocity = 100f;
            bottomJaw.motor = motor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Jump();
    }

    /// <summary>
    /// When collided with water, add score.
    /// </summary>
    /// <param name="collision">Collided object.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            AccumulateScore();

            DayManager.instance.DivePerformed();
            if (DayManager.instance.numDivesRemaining > 0)
            {
                Reset();
            }
        }
    }

    /// <summary>
    /// Add score based on distance travelled and a score multiplier.
    /// </summary>
    private void AccumulateScore()
    {
        if (!hasJumped || myTrainer == null) { return; }

        float distanceTravelled = Vector3.Distance(transform.position, myTrainer.transform.position);
        distanceTravelled *= scoreMultiplier;

        ScoreManager.instance.AddToScore((uint)distanceTravelled);
    }
}
