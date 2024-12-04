using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFoodManager : MonoBehaviour
{
    public List<CropInfo> Crops = new List<CropInfo>();

    public void AddCrop(Crop crop, int amount)
    {
        CropInfo existingCrop = Crops.Find(c => c.Crop == crop);
        if (existingCrop != null)
        {
            existingCrop.Amount += amount;
        }
        else
        {
            Crops.Add(new CropInfo { Crop = crop, Amount = amount });
        }
        Debug.Log("Added " + amount + " units of " + crop.CropName + ". Current amount: " + Crops.Find(c => c.Crop == crop).Amount);
    }

    public bool HasCrop(Crop crop, int requiredAmount)
    {
        CropInfo existingCrop = Crops.Find(c => c.Crop == crop);
        return existingCrop != null && existingCrop.Amount >= requiredAmount;
    }

    public void UseCrop(Crop crop, int amount)
    {
        CropInfo existingCrop = Crops.Find(c => c.Crop == crop);
        if (existingCrop != null && existingCrop.Amount >= amount)
        {
            existingCrop.Amount -= amount;
            Debug.Log("Used " + amount + " units of " + crop.CropName + ". Remaining: " + existingCrop.Amount);

            if (existingCrop.Amount <= 0)
            {
                Crops.Remove(existingCrop);
                Debug.Log("Removed " + crop.CropName + " from inventory as the amount is zero.");
            }
        }
        else
        {
            Debug.Log("Not enough " + crop.CropName + " to use.");
        }
    }
}