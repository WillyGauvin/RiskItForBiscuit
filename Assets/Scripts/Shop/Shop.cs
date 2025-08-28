using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Animator shopPopUp;

    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void OpenShop()
    {
        if (shopPopUp != null && shopPopUp.runtimeAnimatorController != null)
        {
            shopPopUp.SetTrigger("Start");

            StartCoroutine(SetScrollBar());
        }
        else
        {
            Debug.LogWarning("Animator or AnimatorController is missing on " + gameObject.name);
        }
    }

    public void CloseShop()
    {
        shopPopUp.SetTrigger("End");
        StopAllCoroutines();
    }

    IEnumerator SetScrollBar()
    {
        yield return new WaitForSeconds(0.5f);
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
