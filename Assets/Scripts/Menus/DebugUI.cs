using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Very shoddy temporary way of displaying values on screen.
/// </summary>
public class DebugUI : MonoBehaviour
{
    private void Awake()
    {
        var gm = GameManager.instance;
    }

    private void Start()
    {
        ShowScoreCounter();
        ShowMoneyCounter();
        ShowDebtCounter();
        ShowDayCounter();
        ShowDivesCounter();
    }

    private void OnEnable()
    {
        ShowScoreCounter();
        ShowMoneyCounter();
        ShowDebtCounter();
        ShowDayCounter();
        ShowDivesCounter();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            ShowScoreCounter();
            ShowMoneyCounter();
        }

        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            ShowScoreCounter();
        }

        if (Keyboard.current.equalsKey.wasPressedThisFrame)
        {
            ShowMoneyCounter();
        }

        if (Keyboard.current.backslashKey.wasPressedThisFrame)
        {
            ShowDebtCounter();
        }

        if (Keyboard.current.semicolonKey.wasPressedThisFrame)
        {
            ShowDebtCounter();
        }

        if (Keyboard.current.quoteKey.wasPressedThisFrame)
        {
            ShowDebtCounter();
        }

        if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
        {
            ReduceDivesCounter();
        }

        if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
        {
            ShowDayCounter();
        }

        if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
        {
            ShowDayCounter();
        }
    }
#endif

    [SerializeField] TMP_Text dayCounter;
    public void ShowDayCounter()
    {
        dayCounter.text = DayManager.instance.currentDay.ToString();
    }

    [SerializeField] TMP_Text divesCounter;
    public void ShowDivesCounter()
    {
        divesCounter.text = DayManager.instance.numDivesRemaining.ToString();
    }
    public void ReduceDivesCounter()
    {
        divesCounter.text = (DayManager.instance.numDivesRemaining - 1).ToString();
    }

    [SerializeField] TMP_Text scoreCounter;
    public void ShowScoreCounter()
    {
        scoreCounter.text = ScoreManager.instance.currentScore.ToString();
    }

    [SerializeField] TMP_Text moneyCounter;
    public void ShowMoneyCounter()
    {
        moneyCounter.text = ScoreManager.instance.currentMoney.ToString();
    }

    [SerializeField] TMP_Text debtCounter;
    public void ShowDebtCounter()
    {
        debtCounter.text = DebtSystem.instance.DebtRemaining.ToString();
    }
}
