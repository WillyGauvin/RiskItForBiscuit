using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    public enum ActionMap
    {
        Player,
        UI,
    }

    PlayerInput input;
    private InputAction jumpAction;
    private InputAction biteAction;
    private InputAction mousePositionAction;
    private InputAction placeObstacleAction;
    private InputAction exitObstacleAction;
    Dog myDog;

    public Vector2 mousePosition;

    public event Action OnClicked, OnExit;

    private void OnEnable()
    {
        jumpAction.started += ctx => OnPlayerJump(ctx);
        jumpAction.canceled += ctx => OnPlayerJump(ctx);
        biteAction.started += ctx => OnPlayerBite(ctx);
        biteAction.canceled += ctx => OnPlayerBite(ctx);

        mousePositionAction.performed += ctx => OnMouseMove(ctx);

        placeObstacleAction.performed += ctx => OnClicked?.Invoke();
        exitObstacleAction.performed += ctx => OnExit?.Invoke();

    }
    private void OnDisable()
    {
        jumpAction.started -= ctx => OnPlayerJump(ctx);
        jumpAction.canceled -= ctx => OnPlayerJump(ctx);
        biteAction.started -= ctx => OnPlayerBite(ctx);
        biteAction.canceled -= ctx => OnPlayerBite(ctx);

        mousePositionAction.performed -= ctx => OnMouseMove(ctx);


        placeObstacleAction.performed -= ctx => OnClicked?.Invoke();
        exitObstacleAction.performed -= ctx => OnExit?.Invoke();

    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player Controller in the scene.");
        }

        instance = this;

        input = GetComponent<PlayerInput>();
        myDog = GetComponent<Dog>();

        jumpAction = input.actions["Jump"];
        biteAction = input.actions["Bite"];
        mousePositionAction = input.actions["MousePosition"];
        placeObstacleAction = input.actions["PlaceObstacle"];
        exitObstacleAction = input.actions["ExitPlacement"];
    }



    private void OnDestroy()
    {
    }

    private void Start()
    {
    }

    public void SwitchToActionMap(ActionMap map)
    {
        switch (map)
        {
            case ActionMap.Player:
                input.SwitchCurrentActionMap("Game");
                break;
            case ActionMap.UI:
                input.SwitchCurrentActionMap("Menu");
                break;
        }
    }


    void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            GameMenuManager.instance.Pause();
        }
    }

    void OnUnPause(InputValue value)
    {
        if (value.isPressed)
        {
            GameMenuManager.instance.UnPause();
        }
    }

    void OnBeginRun(InputValue value)
    {
        if (value.isPressed)
        {
            myDog.BeginRun();
        }
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            myDog.ChargeJump();
        }
        else if (context.canceled)
        {
            myDog.Jump();
        }
    }

    public void OnPlayerBite(InputAction.CallbackContext context)
    {
        //If the button is pressed, change the motor to be closing
        if (context.started)
        {
            myDog.CloseMouth();
        }
        //If the button is no longer being pressed, change the motor to be opening
        else if (context.canceled)
        {
            myDog.OpenMouth();
        }
    }

    public void OnReset(InputValue value)
    {
        myDog.Reset();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}
