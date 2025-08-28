using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class MinigameController : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    InputAction moveAction;
    InputAction rotateAction;
    InputAction biteAction;
    [SerializeField] MiniDog minigameDog;



    void Awake()
    {
        moveAction = input.actions["Move"];
        rotateAction = input.actions["Rotate"];
        biteAction = input.actions["Bite"];
    }

    void OnEnable()
    {
        moveAction.started += ctx => Move(ctx);
        moveAction.canceled += ctx => Move(ctx);
        biteAction.started += ctx => Bite(ctx);
        biteAction.canceled += ctx => Bite(ctx);
        rotateAction.started += ctx => Rotate(ctx);
        rotateAction.canceled += ctx => Rotate(ctx);
    }

    void OnDisable()
    {
        moveAction.started -= ctx => Move(ctx);
        moveAction.canceled -= ctx => Move(ctx);
        biteAction.started -= ctx => Bite(ctx);
        biteAction.canceled -= ctx => Bite(ctx);
        rotateAction.started -= ctx => Rotate(ctx);
        rotateAction.canceled -= ctx => Rotate(ctx);
    }

    private void Move(InputAction.CallbackContext context)
    {
        minigameDog.Move(context.ReadValue<float>());
    }

    private void Bite(InputAction.CallbackContext context)
    {
        minigameDog.Bite(context);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        minigameDog.Rotate(context.ReadValue<float>());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
