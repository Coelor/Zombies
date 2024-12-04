using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class UIManager : MonoBehaviour
{
    public GameObject FpsController;
    public Camera PlayerCamera;
    public float DetectionRange = 5f;
    public GameObject InfoBox;
    public GameObject InteractableInfoBox;

    private TMP_Text m_HeaderText;
    private TMP_Text m_DescriptionText;
    private string m_CurrentTag;
    private FirstPersonController m_FpsControllerScript;
    private IInteractable m_CurrentInteractable;
    private TMP_Text m_ActionText;
    private TMP_Text m_SecondaryActionText;
    private TMP_Text m_TertiaryActionText;

    private void Start()
    {
        InteractableInfoBox.gameObject.SetActive(false);
        InfoBox.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_FpsControllerScript = FpsController.GetComponent<FirstPersonController>();

        m_HeaderText = InfoBox.transform.Find("Header").GetComponent<TMP_Text>();
        m_DescriptionText = InfoBox.transform.Find("Description").GetComponent<TMP_Text>();
        m_ActionText = InteractableInfoBox.transform.Find("PrimaryInteractionText").GetComponent<TMP_Text>();
        m_SecondaryActionText = InteractableInfoBox.transform.Find("SecondaryOptions/SecondaryInteractionText").GetComponent<TMP_Text>();
        m_TertiaryActionText = InteractableInfoBox.transform.Find("SecondaryOptions/TertiaryInteractionText").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (m_CurrentInteractable == null || m_CurrentTag != "Interactable") return;
            m_CurrentInteractable.OnInteract();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (m_CurrentInteractable == null || m_CurrentTag != "Interactable") return;
            m_CurrentInteractable.OnSecondaryInteract();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (m_CurrentInteractable == null || m_CurrentTag != "Interactable") return;
            m_CurrentInteractable.OnTertiaryInteract();
        }
    }

    private void DetectObject()
    {
        var ray = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, DetectionRange))
        {
            m_CurrentTag = hit.collider.tag;

            if (m_CurrentTag is "Interactable")
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    m_CurrentInteractable = interactable;
                    interactable.OnLookAt();
                }
                else
                {
                    m_ActionText.text = "Press E to interact with " + hit.collider.gameObject.name;
                    InteractableInfoBox.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            InteractableInfoBox.gameObject.SetActive(false);
            InfoBox.gameObject.SetActive(false);
            m_CurrentTag = "";
        }
    }

    private static void ToggleTextObject(TMP_Text textObject)
    {
        textObject.gameObject.SetActive(!string.IsNullOrEmpty(textObject.text));
    }

    public void ShowInteractableInfo(string header, string description, string primaryInteraction, string secondaryInteraction, string tertiaryInteraction)
    {
        m_HeaderText.text = header;
        m_DescriptionText.text = description;
        m_ActionText.text = primaryInteraction;
        m_SecondaryActionText.text = secondaryInteraction;
        m_TertiaryActionText.text = tertiaryInteraction;

        InfoBox.SetActive(true);
        InteractableInfoBox.SetActive(true);
        ToggleTextObject(m_HeaderText);
        ToggleTextObject(m_DescriptionText);
        ToggleTextObject(m_ActionText);
        ToggleTextObject(m_SecondaryActionText);
        ToggleTextObject(m_TertiaryActionText);
    }
}