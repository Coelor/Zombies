using System.Collections;
using System.Collections.Generic;
using Group3d.Notifications;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    public List<Tournament> UpcomingTournaments { get; private set; }
    public int NumOfTournaments = 5;
    public TournamentRank PlayerRank = TournamentRank.RankE;
    public Pet PlayerPet;
    public List<GameObject> OpponentPetPrefabs;
    public GameObject NotePrefab;
    public Transform[] NoteSpawnPositions;
    public string TournamentSceneName;

    // Start is called before the first frame update
    void Start()
    {
        UpcomingTournaments = new List<Tournament>();
        StartCoroutine(GenerateTournaments());
    }

    IEnumerator GenerateTournaments()
    {
        while (true)
        {
            if (UpcomingTournaments.Count < NumOfTournaments)
            {
                foreach (Transform position in NoteSpawnPositions)
                {
                    if (position.childCount == 0)
                    {
                        yield return new WaitForSeconds(Random.Range(10, 20));
                        AddNewTournament(position);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    void AddNewTournament(Transform position)
    {
        TournamentRank rank = (TournamentRank)Random.Range((float)PlayerRank, Mathf.Clamp((int)(PlayerRank + 1), 1, System.Enum.GetValues(typeof(TournamentRank)).Length));

        int randomIndex = Random.Range(0, OpponentPetPrefabs.Count);
        GameObject selectedOpponentPrefab = OpponentPetPrefabs[randomIndex];
        //GameObject opponentPetObject = Instantiate(selectedOpponentPrefab);
        //Pet opponentPetComponent = opponentPetObject.GetComponent<Pet>();
        //opponentPetComponent.Initialize((int)rank);

        float timeToJoin = Random.Range(30, 60);
        Tournament newTournament = new Tournament(rank, null, timeToJoin); //need to change
        UpcomingTournaments.Add(newTournament);

        DisplayTournamentNote(newTournament, position);
        Notifications.Send("Tournament added", NotificationType.Success, null);
        Debug.Log("New Tournament Added: " + newTournament.Rank + " with time left: " + newTournament.TimeLeft + "s");
    }

    void DisplayTournamentNote(Tournament tournament, Transform position)
    {
        GameObject note = Instantiate(NotePrefab, position.position, Quaternion.Euler(0, -90, 0), position);
        note.GetComponent<TournamentNote>().Initialize(tournament, this);
        StartCoroutine(RemoveTournamentAfterTimeout(note, tournament));
    }

    IEnumerator RemoveTournamentAfterTimeout(GameObject note, Tournament tournament)
    {
        yield return new WaitForSeconds(tournament.TimeToJoin);

        if (UpcomingTournaments.Contains(tournament))
        {
            Debug.Log("Tournament Expired: " + tournament.Rank);
            UpcomingTournaments.Remove(tournament);
            Destroy(note);
            //Destroy(tournament.OpponentPet.gameObject);
        }
    }

    public void SignUpForTournament(Tournament tournament)
    {
        if (!UpcomingTournaments.Contains(tournament))
        {
            Debug.LogError("Invalid tournament");
            return;
        }

        tournament.IsPlayerParticipating = true;
        SceneManager.LoadScene(TournamentSceneName);
    }

    IEnumerator ExecuteTournament(Tournament tournament)
    {
        yield return new WaitForSeconds(2f);

        bool playerGoesFirst = PlayerPet.Speed >= tournament.OpponentPet.Speed;
        Pet attacker = playerGoesFirst ? PlayerPet : tournament.OpponentPet;
        Pet defender = playerGoesFirst ? tournament.OpponentPet : PlayerPet;

        while (PlayerPet.Health > 0 && tournament.OpponentPet.Health > 0)
        {
            attacker.Attack(defender);
            yield return new WaitForSeconds(1f);

            (attacker, defender) = (defender, attacker);
        }

        if (PlayerPet.Health > 0)
        {
            RewardPlayer();
        }
        else
        {
            NotifyPlayerLoss();
        }
    }

    void RewardPlayer()
    {
        Debug.Log("Player won the tournament! Reward given.");
    }

    void NotifyPlayerLoss()
    {
        Debug.Log("Player lost the tournament.");
    }
}
