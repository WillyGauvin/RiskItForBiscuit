using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [SerializeField] TMP_Text dayCount;

    [SerializeField] TMP_Text scoreEarned;
    [SerializeField] TMP_Text moneyEarned;

    [SerializeField] TMP_Text totalMoney;

    private void OnEnable()
    {
        SetDayCount();
        SetScoreEarned();
        SetMoneyEarned();
        SetTotalMoney();
    }

    void SetDayCount()
    {
        dayCount.text = "Day " + (DayManager.instance.currentDay - 1).ToString() + " Results";
    }

    void SetScoreEarned()
    {
        scoreEarned.text = "Total Score: " + ScoreManager.instance.totalScore.ToString();
    }

    void SetMoneyEarned()
    {
        moneyEarned.text = "Money Earned: $" + ScoreManager.instance.moneyThisDay.ToString();
    }

    void SetTotalMoney()
    {
        totalMoney.text = "Current Balance: $" + ScoreManager.instance.currentMoney.ToString();
    }
}
