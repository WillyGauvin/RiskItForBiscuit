using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference gameMusic { get; private set; }
    [field: SerializeField] public EventReference menuMusic { get; private set; }

    [field: Header("SFX")]

    [field: Header("Build Menu")]
    [field: SerializeField] public EventReference build_Open { get; private set; }
    [field: SerializeField] public EventReference build_PlaceError { get; private set; }
    [field: SerializeField] public EventReference build_PlaceObject { get; private set; }
    [field: SerializeField] public EventReference build_RemoveBuilding { get; private set; }

    [field: Header("Objects")]
    [field: SerializeField] public EventReference bouncePad { get; private set; }
    [field: SerializeField] public EventReference fireLoop { get; private set; }
    [field: SerializeField] public EventReference waterSplash { get; private set; }


    [field: Header("PlayerSFX")]
    [field: SerializeField] public EventReference player_Bark { get; private set; }
    [field: SerializeField] public EventReference player_CatchFrisbee { get; private set; }
    [field: SerializeField] public EventReference player_Jump { get; private set; }
    [field: SerializeField] public EventReference player_Walk { get; private set; }

    [field: Header("PointSFX")]
    [field: SerializeField] public EventReference countDown { get; private set; }
    [field: SerializeField] public EventReference point_bouncePad { get; private set; }
    [field: SerializeField] public EventReference point_flip { get; private set; }
    [field: SerializeField] public EventReference point_loop { get; private set; }
    [field: SerializeField] public EventReference point_toeTouch { get; private set; }


    [field: Header("Shop")]
    [field: SerializeField] public EventReference shop_buyItem { get; private set; }
    [field: SerializeField] public EventReference shop_enter { get; private set; }
    [field: SerializeField] public EventReference shop_leave { get; private set; }
    [field: SerializeField] public EventReference shop_payLoan { get; private set; }
    [field: SerializeField] public EventReference shop_select { get; private set; }
    [field: SerializeField] public EventReference shop_takeLoan { get; private set; }


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}