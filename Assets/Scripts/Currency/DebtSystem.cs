using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DebtSystem : MonoBehaviour
{
    public static DebtSystem Instance { get; private set; }

    [SerializeField] private DebtDataSO debtDataAsset;
    public static DebtDataSO runtimeData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (runtimeData == null)
        {
            // First time: make the clone
            runtimeData = Instantiate(debtDataAsset);
        }
        else
        {
            foreach (Loan loan in runtimeData.loans)
            {
                loan.ApplyInterest();

                // If any loan goes over 25,000 the player loses.
                if (loan.balance > 25000.0f)
                {
                    GameManager.instance.TriggerGameOver();
                    return;
                }
            }
        }
    }
}
