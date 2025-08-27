using UnityEngine;
using UnityEngine.SceneManagement;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private UpgradeListSO upgradeDataAsset;
    private static UpgradeListSO runtimeData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (runtimeData == null)
        {
            runtimeData = Instantiate(upgradeDataAsset);
        }

        if (SceneManager.GetActiveScene().name == "Dock")
        {
            foreach (UpgradeDataSO upgrade in runtimeData.Upgrades)
            {
                ApplyUpgrade(upgrade);
            }
        }
    }

    /// <summary>
    /// Applies upgrade by calling appropriate manager
    /// </summary>
    /// <param name="upgradeData"></param>
    public void ApplyUpgrade(UpgradeDataSO upgradeData)
    {
        switch (upgradeData.type)
        {
            case UpgradeType.DogStat:
                break;

            case UpgradeType.AbilityUnlock:
                break;

            case UpgradeType.ScoringModifier:
                break;

            case UpgradeType.SceneModifier:
                break;
        }
    }

    /// <summary>
    /// Adds upgrade to upgrade list. Should only be called in Shop Scene
    /// </summary>
    /// <param name="upgradeData"></param>
    public void AddUpgrade(UpgradeDataSO upgradeData)
    {
        runtimeData.AddUpgrade(upgradeData);
    }

    /// <summary>
    /// Removes upgrade from upgrade list. Should only be called in Shop Scene
    /// </summary>
    /// <param name="upgradeData"></param>
    public void RemoveUpgrade(UpgradeDataSO upgradeData)
    {
        runtimeData.RemoveUpgrade(upgradeData);
    }

}
