using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tournament
{
    public TournamentRank Rank { get; private set; }
    public bool IsPlayerParticipating { get; set; }
    public Pet OpponentPet { get; set; }
    public float TimeLeft { get; set; }
    public float TimeToJoin { get; set; }
    
    public Tournament(TournamentRank rank, Pet opponentPet, float timeToJoin)
    {
        Rank = rank;
        TimeToJoin = TimeLeft = timeToJoin;
        OpponentPet = opponentPet;
        IsPlayerParticipating = false;
    }
}
