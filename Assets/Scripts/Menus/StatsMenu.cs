using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [SerializeField] TMP_Text dayCount;

    [SerializeField] TMP_Text scoreEarned;
    [SerializeField] TMP_Text moneyEarned;

    private void OnEnable()
    {
        SetDayCount();
        SetScoreEarned();
        SetMoneyEarned();
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
        moneyEarned.text = "Current Balance: $" + ScoreManager.instance.currentMoney.ToString();
    }
}
