using System.Collections;
using System.Collections.Generic;
using Group3d.Notifications;
using UnityEngine;

public class TournamentNote : MonoBehaviour, IInteractable
{
    private Tournament m_Tournament;
    private TournamentManager m_TournamentManager;
    private UIManager m_UiManager;

    public void Initialize(Tournament tournament, TournamentManager manager)
    {
        m_Tournament = tournament;
        m_TournamentManager = manager;
        m_UiManager = FindObjectOfType<UIManager>();
    }

    public void Update()
    {
        if (m_Tournament.TimeLeft > 0)
        {
            m_Tournament.TimeLeft -= Time.deltaTime;
        }
        else
        {
            m_Tournament.TimeLeft = 0;
        }
    }

    public void OnLookAt()
    {
        if (m_UiManager != null)
        {
            m_UiManager.ShowInteractableInfo(
                "Tournament Rank: " + m_Tournament.Rank.ToString().Insert(4, " "),
                "\nTime Left: " + m_Tournament.TimeLeft.ToString("F0") + "s\n",
                "Press 'E' to sign up for this tournament.",
                "",
                ""
            );
            Highlight(true);
        }
    }

    public void OnInteract()
    {
        Notifications.Send("Starting Tournament", NotificationType.Success, null);
        m_TournamentManager.SignUpForTournament(m_Tournament);
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
    }

    public void OnSecondaryInteract() { }
    public void OnTertiaryInteract() { }
}
