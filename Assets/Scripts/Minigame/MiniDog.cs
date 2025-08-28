using UnityEngine;
using UnityEngine.InputSystem;

public class MiniDog : MonoBehaviour
{
    // Add components here
    [SerializeField] HingeSetter mouthHinge;
    Rigidbody2D rb2d;

    RectTransform rectTransform;

    float moving = 0;

    float rotating = 0;

    [SerializeField] float speed;
    [SerializeField] float rotationalSpeed;

    Vector3 startPos;
    Quaternion startRot;

    bool canBite = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition;
        startRot = rectTransform.localRotation;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        moving = 0;
        rotating = 0;
        rectTransform.localPosition = startPos;
        rectTransform.localRotation = startRot;
        canBite = true;
    }

    public void CantBite()
    {
        canBite = false;
    }

    public void Move(float value)
    {
        moving = value;
    }

    public void Rotate(float value)
    {
        rotating = value;
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(new Vector2(rectTransform.position.x, rectTransform.position.y + (moving * Time.fixedUnscaledDeltaTime * speed)));

        if ((rb2d.rotation < -45 && rotating < 0) || (rb2d.rotation > 35 && rotating > 0))
        {
            return;
        }
        float deltaRotation = rotationalSpeed * Time.fixedUnscaledDeltaTime * rotating;
        rb2d.MoveRotation(rb2d.rotation + deltaRotation);
    }

    public void Bite(InputAction.CallbackContext context)
    {
        if (canBite)
        {
            if (context.started)
            {
                mouthHinge.SetSpeed(-300);
            }
            else if (context.canceled)
            {
                mouthHinge.SetSpeed(300);
            }
        }
    }
}
