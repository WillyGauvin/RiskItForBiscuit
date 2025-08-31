using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.LookDev;

public class Dog : MonoBehaviour
{
    private static Dog _instance;

    public static Dog instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                _instance = FindFirstObjectByType<Dog>();
            }
            return _instance;
        }
    }

    [Header("Running")]
    [SerializeField] float accelerationRate;
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float currentSpeed = 0.0f;

    [Header("Jumping")]
    [SerializeField] float jumpAccelerationRate;
    [SerializeField] float maxJumpForce = 20.0f;
    [SerializeField] float currentJumpForce = 0.0f;
    [SerializeField] float startingJumpForce = 1.0f;

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
    [SerializeField] FrisbeeCatcherAndDetector frisbeeCatchDetection;
    [SerializeField] JumpAndLandDetection jumpLandDetection;

    [Header("AnimationSettings")]
    [SerializeField] DogAnimationManager animationManager;

    [Header("VFX")]
    [SerializeField] GameObject WaterSplashPrefab;


    Vector3 jumpForce;
    public Vector3 startingPos;
    Quaternion startingRot;


    private Rigidbody2D body;
    public Rigidbody2D Body => body;

    [Header("States")]
    public bool isRunning = false;
    public bool isCharging = false;
    public bool hasJumped = false;

    Coroutine JumpCharge;

    public bool isInTutorial = false;

    private void Awake()
    {
        GameManager.instance.SetPlayer(gameObject);

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
            projection.SimulateTrajectory(jumpForce, body.linearVelocity);
        }
    }

    public void Reset()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.Euler(0, 90, 0);
        projection._line.enabled = false;
        body.linearVelocity = Vector3.zero;
        transform.position = startingPos;
        transform.rotation = startingRot;
        isRunning = false;
        isCharging = false;
        currentJumpForce = startingJumpForce;
        currentSpeed = 0.0f;
        jumpForce = Vector3.zero;
        hasJumped = false;
        //detector.Reset();
        if (DockCam != null) { DockCam.gameObject.SetActive(true);}
        frisbeeCatchDetection.Reset();
        animationManager.Reset();
        BuildModeButton.instance.EnableBuildModeButton();
    }

    public void BeginRun()
    {
        if (!isRunning && !BuildModeButton.instance.isInBuildMode && !isInTutorial)
        {
            FollowCam.Follow = transform;
            StartCoroutine(Run());
            projection._line.enabled = true;
            BuildModeButton.instance.DisableBuildModeButton();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.countDown);
        }
    }

    IEnumerator Run()
    {
        isRunning = true;
        while (true)
        {
            currentSpeed += accelerationRate * Time.fixedDeltaTime;

            body.linearVelocity = transform.forward * currentSpeed;

            yield return new WaitForFixedUpdate();
        }
    }

    public void ChargeJump()
    {
        if (!isCharging && isRunning)
        {
            JumpCharge = StartCoroutine(ChargeDogJump());
        }
    }

    public void StopChargeJump()
    {
        StopCoroutine(JumpCharge);
        isCharging = false;
    }
    public void Jump()
    {
        if (hasJumped) return;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.player_Jump);

        hasJumped = true;
        StopAllCoroutines();

        animationManager.ActivateRigidbody();

        StartCoroutine(RealJump());
    }

    IEnumerator RealJump()
    {
        yield return null;
        body.linearVelocity = transform.forward * currentSpeed;
        body.AddForce(jumpForce, ForceMode2D.Impulse);

        // Start tracking score while airborn.
        jumpLandDetection.IncrementScore();

        if (myTrainer != null) { myTrainer.ThrowFrisbee(projection); }
        if (DockCam != null) { DockCam.gameObject.SetActive(false); }
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

    /// <summary>
    /// Can be called from other scripts to begin dog swimming.
    /// </summary>
    public void SwimAfterDive()
    {
        if (!hasJumped) { return; }

        StartCoroutine(BeginSwim());
    }

    /// <summary>
    /// Moves dog towards the left along the water.
    /// </summary>
    /// <returns></returns>
    IEnumerator BeginSwim()
    {
        animationManager.Reset();
        transform.rotation = Quaternion.Euler(0, -90, 0);
        body.linearVelocity = Vector3.zero;
        currentSpeed = 0.0f;
        while (true)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, (accelerationRate / 2.0f) * Time.fixedDeltaTime);

            body.linearVelocity = transform.forward * currentSpeed;

            yield return new WaitForFixedUpdate();
        }
    }


    public void ApplyStatIncrease(UpgradeDataSO upgrade)
    {
        if (upgrade.abilityID == "JumpAccel")
        {
            jumpAccelerationRate *= upgrade.multiplier;
        }

        else if (upgrade.abilityID == "RunAccel")
        {
            accelerationRate *= upgrade.multiplier;
        }

        else if (upgrade.abilityID == "MaxSpeed")
        {
            maxSpeed *= upgrade.multiplier;
        }

        else if (upgrade.abilityID == "MaxJump")
        {
            maxJumpForce *= upgrade.multiplier;
        }
    }


    [Header("Debug")]
    [SerializeField] UpgradeDataSO Jump1, Jump2, Run1, Run2;

    private void FixedUpdate()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            ApplyStatIncrease(Run1);
            Debug.Log("Applied Run1");
        }
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ApplyStatIncrease(Run2);
            Debug.Log("Applied Run2");
        }
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            ApplyStatIncrease(Jump1);
            Debug.Log("Applied Jump1");
        }
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ApplyStatIncrease(Jump2);
            Debug.Log("Applied Jump2");
        }
    }

    public void Landed()
    {
        if (WaterSplashPrefab)
        {
            Instantiate(WaterSplashPrefab, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
        }
        //FollowCam.Follow = null;
    }
}
