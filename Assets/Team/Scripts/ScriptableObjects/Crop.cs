using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "IdleGame/Crop")]
public class Crop : ScriptableObject
{
    [SerializeField] private string m_CropName;
    [SerializeField] private int m_UpgradeCost = 5;
    [SerializeField] private int m_CurrentLevel = 1;
    [SerializeField] private int m_FoodPerSecond = 1;
    [SerializeField] private AttributeBoostEntry[] m_AttributeBoosts = new AttributeBoostEntry[4]{
        new AttributeBoostEntry { AttributeType = AttributeType.Strength, BoostAmount = 0 },
        new AttributeBoostEntry { AttributeType = AttributeType.Defense, BoostAmount = 0 },
        new AttributeBoostEntry { AttributeType = AttributeType.Speed, BoostAmount = 0 },
        new AttributeBoostEntry { AttributeType = AttributeType.Health, BoostAmount = 0 }
    };

    public string CropName { get => m_CropName; set => m_CropName = value; }
    public int UpgradeCost { get => m_UpgradeCost; set => m_UpgradeCost = value; }
    public int CurrentLevel { get => m_CurrentLevel; set => m_CurrentLevel = value; }
    public int FoodPerSecond { get => m_FoodPerSecond; set => m_FoodPerSecond = value; }
    public AttributeBoostEntry[] AttributeBoosts { get => m_AttributeBoosts; set => m_AttributeBoosts = value; }

    private void OnEnable()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        UpgradeCost = 5;
        FoodPerSecond = 1;
        CurrentLevel = 1;
    }

    public void UpgradeCrop()
    {
        if (CurrentLevel > 0)
        {
            CurrentLevel++;
            FoodPerSecond += 1;
            UpgradeCost *= 2;
            Debug.Log("Crop upgraded to level " + CurrentLevel + ". New food per second: " + FoodPerSecond + ", New upgrade cost: " + UpgradeCost);
        }
    }
}
