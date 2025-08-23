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

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player Controller in the scene.");
        }

        instance = this;

        input = GetComponent<PlayerInput>();
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
}
