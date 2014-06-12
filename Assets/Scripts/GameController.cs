using UnityEngine;

public class GameController : MonoBehaviour 
{
    public void Start()
    {
        ConsoleCommandsRepository ccr = ConsoleCommandsRepository.Instance;
        ccr.RegisterCommand("endRound", EndRound);
    }

    /// <summary>
    /// End the current round.
    /// </summary>
    public void EndRound(params string[] args)
    {
        Utils.BroadcastMessageAll("OnRoundEnd");
        StartRound();
    }

    public void StartRound()
    {
        Utils.BroadcastMessageAll("OnRoundStart");
    }
}
