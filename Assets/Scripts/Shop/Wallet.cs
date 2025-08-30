using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Wallet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI money;

    private void Awake()
    {
        ScoreManager.instance.UpdateMoney.AddListener(UpdateWallet);
        UpdateWallet();
    }
    private void OnDestroy()
    {
        ScoreManager.instance.UpdateMoney.RemoveListener(UpdateWallet);
    }

    private void UpdateWallet()
    {
        money.text = "$ " + ScoreManager.instance.currentMoney.ToString();
    }
}
