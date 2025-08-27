using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[CreateAssetMenu]
public class TrainerDataSO : ScriptableObject
{
    public List<DogTrainer> trainers;
    public DogTrainer activeTrainer;
}


[Serializable]
public class DogTrainer
{
    [field: SerializeField] public UpgradeDataSO activatedUpgrade;

    [field: SerializeField] public List<UpgradeDataSO> permanentUpgrades;

    [field: SerializeField] public int price;

    [field: SerializeField] public bool isUnlocked = false;

    [field: SerializeField] public Image icon;

    [field: SerializeField] public string name;
}