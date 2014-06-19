using ExitGames.Client.Photon;
using UnityEngine;

public class GameController : Photon.MonoBehaviour 
{
    public bool IsCurrent = false;
    public bool IsEnded = false;
    public bool IsWinner = false;

    public void Start()
    {
        // Setup the photon connection.
        PhotonNetwork.ConnectUsingSettings("1");
    }

    public void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions();
        ro.maxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("main", ro, TypedLobby.Default);
    }

    public void OnJoinedRoom()
    {
        IsCurrent = PhotonNetwork.isMasterClient;
    }

    /// <summary>
    /// End the current round.
    /// </summary>
    public void EndRound()
    {
        IsCurrent = false;
        Utils.BroadcastMessageAll("OnRoundEnd");
        if (PhotonNetwork.otherPlayers.Length > 0)
        {
            photonView.RPC("StartRound", PhotonNetwork.otherPlayers[0]);
        }
        else
        {
            StartRound();
        }
    }

    [RPC]
    public void StartRound()
    {
        IsCurrent = true;
        Utils.BroadcastMessageMine("OnRoundStart");
    }
}
