using UnityEngine;

public class TrainerShop : Shop
{
    public static TrainerShop Instance { get; private set; }

    [SerializeField] private TrainerDataSO trainerDataAsset;
    private static TrainerDataSO runtimeData;

    [SerializeField] private TrainerItemUI trainerPrefab;
    [SerializeField] private GameObject shopContent;

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
        }

        LoadShop();
    }
    
    private void LoadShop()
    {
        foreach(DogTrainer trainer in runtimeData.trainers)
        {
            TrainerItemUI item = Instantiate(trainerPrefab, shopContent.transform);
            item.Init(trainer);
        }
    }
}
