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
        BroadcastMessageAll("OnRoundEnd");
        StartRound();
    }

    public void StartRound()
    {
        BroadcastMessageAll("OnRoundStart");
    }

    /// <summary>
    /// Broadcast a message to all game objects in the scene.
    /// </summary>
    /// <param name="message"></param>
    private void BroadcastMessageAll(string message)
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SendMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }
}
