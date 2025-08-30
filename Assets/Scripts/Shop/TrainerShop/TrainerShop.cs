using UnityEngine;
using System.Collections.Generic;

public class TrainerShop : Shop
{
    public static TrainerShop Instance { get; private set; }

    [SerializeField] private TrainerDataSO trainerDataAsset;
    private static TrainerDataSO runtimeData;

    [SerializeField] private TrainerItemUI trainerPrefab;
    [SerializeField] private GameObject shopContent;

    private List<TrainerItemUI> trainerUIs = new List<TrainerItemUI>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (runtimeData == null)
        {
            // First time: make the clone
            runtimeData = Instantiate(trainerDataAsset);

            foreach (DogTrainer trainer in runtimeData.trainers)
            {
                // Clone each upgrade so we don't touch the original
                if (trainer.permanentUpgrades != null)
                {
                    for (int i = 0; i < trainer.permanentUpgrades.Count; i++)
                    {
                        trainer.permanentUpgrades[i] = Instantiate(trainer.permanentUpgrades[i]);
                    }
                }

                // clone the activated upgrade if it exists
                if (trainer.activatedUpgrade != null)
                {
                    trainer.activatedUpgrade = Instantiate(trainer.activatedUpgrade);
                }
            }
        }

        LoadShop();
    }
    
    private void LoadShop()
    {
        foreach(DogTrainer trainer in runtimeData.trainers)
        {
            TrainerItemUI item = Instantiate(trainerPrefab, shopContent.transform);
            item.Init(trainer, (runtimeData.activeTrainer == trainer) ? true : false);
            trainerUIs.Add(item);
        }
    }

    public void SetTrainer(DogTrainer trainer)
    {
        //Toggle all trainers off except mine.
        if (trainer != null)
        {
            TurnOffAllToggles(trainer);
        }

        //Remove old trainers ability
        if (runtimeData.activeTrainer != null)
        {
            if (runtimeData.activeTrainer.activatedUpgrade != null)
            {
                UpgradeManager.Instance.RemoveUpgrade(runtimeData.activeTrainer.activatedUpgrade);
            }
            runtimeData.activeTrainer = null;
        }

        runtimeData.activeTrainer = trainer;
        if (runtimeData.activeTrainer != null)
        {
            if (runtimeData.activeTrainer.activatedUpgrade != null)
            {
                UpgradeManager.Instance.AddUpgrade(runtimeData.activeTrainer.activatedUpgrade);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_equipTrainer);

            }
        }
    }

    public void TurnOffAllToggles(DogTrainer exception)
    {
        foreach(TrainerItemUI trainer in trainerUIs)
        {
            if (trainer.myTrainer != exception)
            {
                trainer.ToggleOff();
            }
        }
    }
}
