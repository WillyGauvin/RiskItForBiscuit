using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class GameMask
{
    public Vector2 maskPosition;
    public Vector2 maskSize;
    public string sentence;
}

public class GameTutorial : MonoBehaviour
{
    public Image mask;
    public Image overlay;
    public TextMeshProUGUI text;
    public List<GameMask> sentences;
    [SerializeField] PlayerInput input;

    PlayerInput myInput;

    [SerializeField] BoughItemStart boughtItemTutorial;
    int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myInput = GetComponent<PlayerInput>();
        if (LevelLoader.tutorialActive)
        {
            mask.enabled = true;
            overlay.enabled = true;
            text.enabled = true;
            input.SwitchCurrentActionMap("Tutorial");
            LevelLoader.tutorialActive = false;

            text.text = sentences[index].sentence;
            mask.transform.localScale = sentences[index].maskSize;
            mask.transform.localPosition = sentences[index].maskPosition;
        }
    }

    public void OnClick()
    {
        if (index < sentences.Count - 1)
        {
            index += 1;
            text.text = sentences[index].sentence;
            mask.transform.localScale = sentences[index].maskSize;
            mask.transform.localPosition = sentences[index].maskPosition;
        }
        else
        {
            if (ObstacleManager.buildTutorialNeeded && BoughItemStart.boughItemDone == false)
            {
                //Start build tutorial
                BoughItemStart.boughItemDone = true;
                BoughItemStart.instance.StartTutorial();
            }
            else
            {
                input.SwitchCurrentActionMap("Game");
            }
            this.gameObject.SetActive(false);
        }
    }
}
