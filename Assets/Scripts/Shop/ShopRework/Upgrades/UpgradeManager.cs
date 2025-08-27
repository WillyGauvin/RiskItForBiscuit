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
                Debug.Log($"Applied Dog Stat: {upgradeData.abilityID}");
                break;

            case UpgradeType.AbilityUnlock:

                Debug.Log($"Applied Ability Unlock: {upgradeData.abilityID}");
                break;

            case UpgradeType.ScoringModifier:
                Debug.Log($"Applied Scoring Modifier: {upgradeData.abilityID}");
                break;

            case UpgradeType.SceneModifier:
                Debug.Log($"Applied Scene Modifier: {upgradeData.abilityID}");
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
        Debug.Log($"Added Upgrade: {upgradeData.abilityID}");
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
