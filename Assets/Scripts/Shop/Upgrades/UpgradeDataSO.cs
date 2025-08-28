using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;

[CreateAssetMenu]
public class UpgradeDataSO : ScriptableObject
{
    [Header("General")]

    [SerializeField] public UpgradeType type;

    [SerializeField] public string abilityID;

    [SerializeField] public string description;

    [SerializeField] public int price;

    [SerializeField] public bool isUnlocked = false;

    [SerializeField] public float multiplier;
}

public enum UpgradeType
{
    DogStat,
    SceneModifier,
    ScoringModifier,
    AbilityUnlock,
}