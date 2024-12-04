using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : MonoBehaviour, IInteractable
{
    [SerializeField] private int m_Strength = 1;
    [SerializeField] private int m_Defense = 1;
    [SerializeField] private int m_Health = 1;
    [SerializeField] private int m_Speed = 1;
    [SerializeField] private string m_Type = "Bear";
    [SerializeField] private string m_PetName;

    public int Strength { get => m_Strength; set => m_Strength = value; }
    public int Defense { get => m_Defense; set => m_Defense = value; }
    public int Health { get => m_Health; set => m_Health = value; }
    public int Speed { get => m_Speed; set => m_Speed = value; }
    public string Type { get => m_Type; set => m_Type = value; }
    public string PetName { get => m_PetName; set => m_PetName = value; }

    private UIManager m_UiManager;
    private PlayerFoodManager m_FoodManager;
    private PetController m_PetController;
    private Crop m_CurrentCrop;
    private Animator m_PetAnimator;

    private void Start()
    {
        if (string.IsNullOrEmpty(PetName))
        { 
            PetName = Type;
        }
        m_UiManager = FindObjectOfType<UIManager>();
        m_FoodManager = FindObjectOfType<PlayerFoodManager>();
        m_PetController = GetComponent<PetController>();

        if (m_FoodManager.Crops.Count > 0)
        {
            m_CurrentCrop = m_FoodManager.Crops[0].Crop;
        }

    }

    public void Initialize(int rank)
    {
        Speed = Random.Range(rank * 5, rank * 10);
        Strength = Random.Range(rank * 5, rank * 10);
        Defense = Random.Range(rank * 3, rank * 7);
        Health = Random.Range(rank * 10, rank * 20);
    }

    public void OnLookAt()
    {
        Debug.Log("Looking at " + PetName);

        if (m_PetController != null)
        {
            m_PetController.enabled = false;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat("forward", 0);
            animator.SetBool("isIdle", true);
        }

        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }

        Vector3 directionToPlayer = (m_UiManager.PlayerCamera.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
        m_UiManager.ShowInteractableInfo(
                  PetName.ToString(),
                 "Strength " + Strength +
                            "\nDefense " + Defense +
                            "\nSpeed" + Speed +
                            "\nHealth" + Health,
                "Press 'E' to feed" + m_CurrentCrop,
                  "Press 'Q' to change crop",
                  ""
                  );
        Highlight(true);
    }

    public void OnInteract()
    {
        if (m_FoodManager == null) return;
        m_UiManager.ShowInteractableInfo(
            "",
            "",
            "Feeding" + PetName,
            "",
            ""
        );
        Debug.Log("Feeding " + PetName);
        if (m_CurrentCrop == null)
        {
            Debug.Log("No crop selected for feeding.");
            return;
        }

        Feed(m_CurrentCrop);
    }

    public void OnSecondaryInteract()
    {
        if (m_FoodManager.Crops.Count > 0)
        {
            int currentIndex = m_FoodManager.Crops.FindIndex(c => c.Crop == m_CurrentCrop);
            int nextIndex = (currentIndex + 1) % m_FoodManager.Crops.Count;
            m_CurrentCrop = m_FoodManager.Crops[nextIndex].Crop;
            Debug.Log("Current crop for feeding changed to: " + m_CurrentCrop.CropName);
        }
        else
        {
            Debug.Log("No crops available to change for feeding.");
        }
    }
    public void OnTertiaryInteract(){} //Maybe pet name change idk

    public void Feed(Crop crop)
    {
        CropInfo cropInfo = m_FoodManager.Crops.Find(c => c.Crop == crop);
        if (cropInfo == null || cropInfo.Amount < crop.UpgradeCost)
        {
            Debug.Log("Not enough of " + crop.CropName + " to feed.");
            return;
        }

        m_FoodManager.UseCrop(crop, crop.UpgradeCost);

        Strength += crop.AttributeBoosts[0].BoostAmount;
        Defense += crop.AttributeBoosts[1].BoostAmount;
        Speed += crop.AttributeBoosts[2].BoostAmount;
        Health += crop.AttributeBoosts[3].BoostAmount;

        Debug.Log($"Fed {PetName} with {crop.CropName}. Stats updated: Strength={Strength}, Defense={Defense}, Speed={Speed}, Health={Health}");
    }

    private void Highlight(bool highlight)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = highlight ? Color.yellow : Color.white;
        }
    }

    private void OnMouseExit()
    {
        Highlight(false);

        if (m_PetController != null)
        {
            m_PetController.enabled = true;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat("forward", .1f);
            animator.SetBool("isIdle", false);
        }

        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
    }

    public void Attack(Pet defender)
    {
        int damage = Strength - defender.Defense;
        damage = damage > 0 ? damage : 1;
        defender.Health -= damage;

        Debug.Log($"{PetName} attacked {defender.PetName} for {damage} damage. {defender.PetName} now has {defender.Health} health.");
    }
}