using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu]
public class UpgradeListSO : ScriptableObject
{
    public List<UpgradeDataSO> Upgrades = new List<UpgradeDataSO>();
    
    public void AddUpgrade(UpgradeDataSO upgrade)
    {
        Upgrades.Add(upgrade);
    }

    public void RemoveUpgrade(UpgradeDataSO upgrade)
    {
        bool didRemove = Upgrades.Remove(upgrade);

        if (!didRemove) Debug.LogError($"Upgrade was not found in list: {upgrade.abilityID}");
    }
}
