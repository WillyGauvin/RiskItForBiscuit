using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [Header("Day Information")]
    [SerializeField] TMP_Text dayCounter;
    [SerializeField] TMP_Text divesCounter;

    [Header("Currency/Score Information")]
    [SerializeField] TMP_Text scoreCounter;
    [SerializeField] TMP_Text moneyCounter;
    [SerializeField] TMP_Text debtCounter;

    private void OnEnable()
    {
        ScoreManager.instance.UpdateTotalScore.AddListener(UpdateScoreCounter);
        ScoreManager.instance.UpdateMoney.AddListener(UpdateMoneyCounter);

        DayManager.instance.UpdateDayCount.AddListener(UpdateDayCounter);
        DayManager.instance.UpdateDivesCount.AddListener(UpdateDivesCounter);

        UpdateUIElements();
    }

    private void OnDisable()
    {
        ScoreManager.instance.UpdateTotalScore.RemoveListener(UpdateScoreCounter);
        ScoreManager.instance.UpdateMoney.RemoveListener(UpdateMoneyCounter);

        DayManager.instance.UpdateDayCount.RemoveListener(UpdateDayCounter);
        DayManager.instance.UpdateDivesCount.RemoveListener(UpdateDivesCounter);
    }

    private void OnDestroy()
    {
        ScoreManager.instance.UpdateTotalScore.RemoveListener(UpdateScoreCounter);
        ScoreManager.instance.UpdateMoney.RemoveListener(UpdateMoneyCounter);
        DayManager.instance.UpdateDayCount.RemoveListener(UpdateDayCounter);
        DayManager.instance.UpdateDivesCount.RemoveListener(UpdateDivesCounter);
    }

    void UpdateUIElements()
    {
        UpdateDayCounter();
        UpdateDivesCounter();
        UpdateScoreCounter();
        UpdateMoneyCounter();
    }

    void UpdateDayCounter()
    {
        if (dayCounter) { dayCounter.text = "Day " + DayManager.instance.currentDay.ToString(); }
    }

    void UpdateDivesCounter()
    {
        if (divesCounter) { divesCounter.text = "Dives: " + DayManager.instance.numDivesRemaining.ToString(); }
    }

    void UpdateScoreCounter()
    {
        if (scoreCounter) { scoreCounter.text = "Score: " + ScoreManager.instance.totalScore.ToString(); }
    }

    void UpdateMoneyCounter()
    {
        if (moneyCounter) { moneyCounter.text = "$" + ScoreManager.instance.currentMoney.ToString(); }
    }
}
