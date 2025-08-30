using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public enum Music_States
{
    newday_street = 0,
    shop_01,
    shop_02,
    loan_shark,
    dock_dive,
    pause,
    day_over,
    game_over,
    game_win,
}

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances = new List<EventInstance>();
    private List<StudioEventEmitter> eventEmitters = new List<StudioEventEmitter>();

    private EventInstance ambienceEventInstance;
    private EventInstance menuMusicEventInstance;
    private EventInstance gameMusicEventInstance;

    private static AudioManager thisInstance;

    private Music_States previousState;

    public static AudioManager instance
    {
        get
        {
            if (!thisInstance)
            {
                // Spawns GameManager prefab and sets component reference.
                Debug.Log("Instance was null");
                GameObject manager = Instantiate(Resources.Load<GameObject>("Managers/AudioManager"));
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    private void Awake()
    {
        if (thisInstance == null)
        {
            thisInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/music");
        ambienceBus = RuntimeManager.GetBus("bus:/ambience");
        sfxBus = RuntimeManager.GetBus("bus:/sfx");

        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        LoadPlayerPrefs();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void Start()
    {
        LoadPlayerPrefs();

        Scene current = SceneManager.GetActiveScene();

        // Initialize music/ambience for the first scene
        if (current.name == "MainMenu")
            InitializeMenuMusic(FMODEvents.instance.menuMusic);
        else
        {
            InitializeGameMusic(FMODEvents.instance.gameMusic);
            InitializeAmbience(FMODEvents.instance.ambience);

            if (current.name == "Dock")
            {
                SetMusicArea(Music_States.dock_dive);
                SetAmbienceParameter("ambience_transition", 1.0f);
            }
            else if (current.name == "Shop")
            {
                SetMusicArea(Music_States.newday_street);
                SetAmbienceParameter("ambience_transition", 0.0f);
            }
        }

    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", ambienceVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    void LoadPlayerPrefs()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1.0f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);

        ambienceEventInstance.start();

        SetAmbienceParameter("ambience_transition", (SceneManager.GetActiveScene().name == "Shop") ? 0.0f : 1.0f);
    }

    private void InitializeMenuMusic(EventReference musicEventReference)
    {
        menuMusicEventInstance = CreateInstance(musicEventReference);

        menuMusicEventInstance.start();
    }

    private void InitializeGameMusic(EventReference musicEventReference)
    {
        gameMusicEventInstance = CreateInstance(musicEventReference);
        gameMusicEventInstance.start();

        if (SceneManager.GetActiveScene().name == "Dock")
        {
            SetMusicArea(Music_States.dock_dive);
        }
        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            SetMusicArea(Music_States.newday_street);
        }
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }
    public void SetMusicArea(Music_States area)
    {
        gameMusicEventInstance.getParameterByName("music_ingame", out float currentState);
        previousState = (Music_States)(int)currentState;

        gameMusicEventInstance.setParameterByName("music_ingame", (float)area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    public void ForceLoad()
    {

    }

    public void OnGamePause()
    {
        SetMusicArea(Music_States.pause);
        ambienceEventInstance.setPaused(true);
    }

    public void OnGameUnPause()
    {
        SetMusicArea(previousState);
        ambienceEventInstance.setPaused(false);
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if (newScene.name == "GameVictory")
        {
            SetMusicArea(Music_States.game_win);
        }
        else if (newScene.name == "GameOver")
        {
            SetMusicArea(Music_States.game_over);
        }

        if (newScene.name == "MainMenu")
        {
            CleanUp();
            InitializeMenuMusic(FMODEvents.instance.menuMusic);
        }

        else if (!gameMusicEventInstance.isValid() &&  newScene.name != "MainMenu")
        {
            CleanUp();

            InitializeGameMusic(FMODEvents.instance.gameMusic);
            InitializeAmbience(FMODEvents.instance.ambience);

            if (newScene.name == "Dock")
            {
                SetMusicArea(Music_States.dock_dive);
                SetAmbienceParameter("ambience_transition", 1.0f);
            }
            else if (newScene.name == "Shop")
            {
                SetMusicArea(Music_States.newday_street);
                SetAmbienceParameter("ambience_transition", 0.0f);
            }
        }
        else
        {
            if (newScene.name == "Dock")
            {
                Debug.Log("Going To Dock Scene");
                SetMusicArea(Music_States.dock_dive);
                SetAmbienceParameter("ambience_transition", 1.0f);
            }
            else if (newScene.name == "Shop")
            {
                Debug.Log("Going To Shop Scene");
                SetMusicArea(Music_States.newday_street);
                SetAmbienceParameter("ambience_transition", 0.0f);
            }
        }
    }
}