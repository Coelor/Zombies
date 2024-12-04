using System.Collections;
using System.Collections.Generic;
using Group3d.Notifications;
using UnityEngine;

public class Farm : MonoBehaviour, IInteractable
{
    [SerializeField] private Crop m_CurrentCrop;
    [SerializeField] private int m_CurrentCapacity = 0;
    [SerializeField] private int m_MaxCapacity = 10;
    [SerializeField] private int m_UpgradeCost = 20;

    public List<Crop> ListOfCrops;

    public Crop CurrentCrop { get => m_CurrentCrop; set => m_CurrentCrop = value; }
    public int CurrentCapacity { get => m_CurrentCapacity; set => m_CurrentCapacity = value; }
    public int MaxCapacity { get => m_MaxCapacity; set => m_MaxCapacity = value; }
    public int UpgradeCost { get => m_UpgradeCost; set => m_UpgradeCost = value; }

    private PlayerFoodManager m_PlayerFoodManager;
    private UIManager m_UiManager;

    private void Start()
    {
        if (CurrentCrop == null)
        {
            Debug.LogError("No crop assigned to the farm!");
        }
        m_PlayerFoodManager = FindObjectOfType<PlayerFoodManager>();
        m_UiManager = FindObjectOfType<UIManager>();

        StartCoroutine(PassiveFoodGain());
    }

    public void OnLookAt()
    {
        Debug.Log("Looking at the farm growing: " + CurrentCrop.CropName);

        if (m_UiManager != null)
        {
            m_UiManager.ShowInteractableInfo(
                "Farm (" + CurrentCrop.CropName.ToString() + ")",
                "Current Capacity: " + CurrentCapacity + "/" + MaxCapacity + "\n" +
                            "Upgrade Cost: " + UpgradeCost,
                "Press 'E' to harvest",
                "Press 'Q' to upgrade",
                "Press 'R' to change crop"
            );
        }
    }

    public void OnInteract()
    {
        HarvestCrop();
    }

    public void OnSecondaryInteract()
    {
        UpgradeFarmCapacity();
    }

    public void OnTertiaryInteract()
    {
        ChangeCrop();
    }

    private void HarvestCrop()
    {
        if (CurrentCapacity > 0)
        {
            m_PlayerFoodManager.AddCrop(CurrentCrop, CurrentCapacity);
            Debug.Log("Harvested " + CurrentCapacity + " units of " + CurrentCrop.CropName);
            CurrentCapacity = 0;

            StartCoroutine(PassiveFoodGain());
        }
        else
        {
            Debug.Log("The farm is empty, nothing to harvest.");
        }
    }

    private IEnumerator PassiveFoodGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            CurrentCapacity += 1;
            Debug.Log("Total Amount of food now: " + CurrentCapacity);

            if (CurrentCapacity >= MaxCapacity)
            {
                Debug.Log("Max capacity reached, stopping passive food gain.");
                Notifications.Send("Farm (" + m_CurrentCrop.CropName + ") full!", NotificationType.Error, null);
                yield break;
            }
        }
    }

    private void UpgradeFarmCapacity()
    {
        if (m_PlayerFoodManager.HasCrop(CurrentCrop, UpgradeCost))
        {
            m_PlayerFoodManager.UseCrop(CurrentCrop, UpgradeCost);
            MaxCapacity += 5;
            UpgradeCost *= 2;
            Debug.Log("Farm upgraded! New max capacity: " + MaxCapacity);
        }
        else
        {
            Debug.Log("Not enough of " + CurrentCrop.CropName + " to upgrade the farm. Upgrade cost: " + UpgradeCost);
        }
    }

    private void ChangeCrop()
    {
        HarvestCrop();

        if (ListOfCrops.Count > 0)
        {
            int currentIndex = ListOfCrops.IndexOf(m_CurrentCrop);
            int nextIndex = (currentIndex + 1) % ListOfCrops.Count;
            m_CurrentCrop = ListOfCrops[nextIndex];
            Debug.Log("Crop changed to: " + m_CurrentCrop.CropName);
        }
        else
        {
            Debug.Log("No crops available to change.");
        }
    }
}