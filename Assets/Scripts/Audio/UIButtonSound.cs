using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundManager : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.ui_Click);
    }
}