using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public enum ShopType
{
    Obstacle,
    Trainer,
    Debt,
}

public class Shop : MonoBehaviour
{
    public Animator shopPopUp;

    [SerializeField] private ShopType shopType;

    [SerializeField] string id;

    [SerializeField] ShopTutorial tutorial;

    [SerializeField] TutorialManager tutorialManager;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void OpenShop()
    {
        if (shopPopUp != null && shopPopUp.runtimeAnimatorController != null)
        {
            if (tutorialManager.CheckIfFirstTime(id))
            {
                Debug.Log("Tutorial Started");
                tutorial.StartTutorial();
            }
            shopPopUp.SetTrigger("Start");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_enter);
            AudioManager.instance.SetAmbienceParameter("ambience_transition", 0.5f);
            switch(shopType)
            {
                case ShopType.Obstacle:
                    AudioManager.instance.SetMusicArea(Music_States.shop_01);
                    break;
                case ShopType.Trainer:
                    AudioManager.instance.SetMusicArea(Music_States.shop_02);
                    break;
                case ShopType.Debt:
                    AudioManager.instance.SetMusicArea(Music_States.loan_shark);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Animator or AnimatorController is missing on " + gameObject.name);
        }
    }

    public void CloseShop()
    {
        shopPopUp.SetTrigger("End");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_leave);
        AudioManager.instance.SetAmbienceParameter("ambience_transition", 0.0f);
        AudioManager.instance.SetMusicArea(Music_States.newday_street);


        StopAllCoroutines();
    }
}
