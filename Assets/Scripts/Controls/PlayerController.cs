using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    Dog myDog;

    private void OnEnable()
    {
        jumpAction.started += ctx => OnPlayerJump(ctx);
        jumpAction.canceled += ctx => OnPlayerJump(ctx);
        biteAction.started += ctx => OnPlayerBite(ctx);
        biteAction.canceled += ctx => OnPlayerBite(ctx);

    }
    private void OnDisable()
    {
        jumpAction.started -= ctx => OnPlayerJump(ctx);
        jumpAction.canceled -= ctx => OnPlayerJump(ctx);
        biteAction.started += ctx => OnPlayerBite(ctx);
        biteAction.canceled += ctx => OnPlayerBite(ctx);
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
}
