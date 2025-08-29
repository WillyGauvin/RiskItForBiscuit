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
    private InputAction backCalfAction;
    private InputAction backThighAction;
    private InputAction frontCalfAction;
    private InputAction frontThighAction;
    private InputAction wagTailAction;
    LegControls legControls;

    Dog myDog;
    private InputAction mousePositionAction;
    private InputAction placeObstacleAction;
    private InputAction exitObstacleAction;

    public Vector2 mousePosition;

    public event Action OnClicked, OnExit;

    private void OnEnable()
    {
        jumpAction.started += ctx => OnPlayerJump(ctx);
        jumpAction.canceled += ctx => OnPlayerJump(ctx);
        biteAction.started += ctx => OnPlayerBite(ctx);
        biteAction.canceled += ctx => OnPlayerBite(ctx);


        backCalfAction.started += ctx => OnBackCalfContraction(ctx);
        backCalfAction.canceled += ctx => OnBackCalfContraction(ctx);
        frontCalfAction.started += ctx => OnFrontCalfContraction(ctx);
        frontCalfAction.canceled += ctx => OnFrontCalfContraction(ctx);
        backThighAction.started += ctx => OnBackThighContraction(ctx);
        backThighAction.canceled += ctx => OnBackThighContraction(ctx);
        frontThighAction.started += ctx => OnFrontThighContraction(ctx);
        frontThighAction.canceled += ctx => OnFrontThighContraction(ctx);
        wagTailAction.started += ctx => OnTailWag(ctx);
        wagTailAction.canceled += ctx => OnTailWag(ctx);

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


        backCalfAction.started -= ctx => OnBackCalfContraction(ctx);
        backCalfAction.canceled -= ctx => OnBackCalfContraction(ctx);
        frontCalfAction.started -= ctx => OnFrontCalfContraction(ctx);
        frontCalfAction.canceled -= ctx => OnFrontCalfContraction(ctx);
        backThighAction.started -= ctx => OnBackThighContraction(ctx);
        backThighAction.canceled -= ctx => OnBackThighContraction(ctx);
        frontThighAction.started -= ctx => OnFrontThighContraction(ctx);
        frontThighAction.canceled -= ctx => OnFrontThighContraction(ctx);
        wagTailAction.started -= ctx => OnTailWag(ctx);
        wagTailAction.canceled -= ctx => OnTailWag(ctx);

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
        //Mine
        legControls = GetComponent<LegControls>();
        //End of mine

        jumpAction = input.actions["Jump"];
        biteAction = input.actions["Bite"];

        //Stuff I added
        backCalfAction = input.actions["BackCalf"];
        backThighAction = input.actions["BackThigh"];
        frontCalfAction = input.actions["FrontCalf"];
        frontThighAction = input.actions["FrontThigh"];
        wagTailAction = input.actions["WagTail"];
        //End

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
            GameMenuManager.Instance.Pause();
        }
    }

    void OnUnPause(InputValue value)
    {
        if (value.isPressed)
        {
            GameMenuManager.Instance.UnPause();
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
            myDog.StopChargeJump();
        }
    }

    public void OnPlayerBite(InputAction.CallbackContext context)
    {
        //If the button is pressed, change the motor to be closing
        if (context.started)
        {
            //myDog.CloseMouth();
        }
        //If the button is no longer being pressed, change the motor to be opening
        else if (context.canceled)
        {
            //myDog.OpenMouth();
        }
    }

    public void OnReset(InputValue value)
    {
        myDog.Reset();
    }


    public void OnBackCalfContraction(InputAction.CallbackContext context)
    {
        legControls.OnContractBackCalf(context);
    }

    public void OnBackThighContraction(InputAction.CallbackContext context)
    {
        legControls.OnContractBackThigh(context);
    }

    public void OnFrontCalfContraction(InputAction.CallbackContext context)
    {
        legControls.OnContractFrontCalf(context);
    }

    public void OnFrontThighContraction(InputAction.CallbackContext context)
    {
        legControls.OnContractFrontThigh(context);
    }

    public void OnTailWag(InputAction.CallbackContext context)
    {
        legControls.OnTailWag(context);
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}
