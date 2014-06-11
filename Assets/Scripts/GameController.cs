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
    private void EndRound(params string[] args)
    {
        BroadcastMessageAll("OnRoundEndMove");
        BroadcastMessageAll("OnRoundEndProcess");
        BroadcastMessageAll("OnRoundEndDie");
    }

    private void BroadcastMessageAll(string message)
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SendMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// This is the first end round step: movement.
    /// At this stage all objects may do is move to their new location.
    /// </summary>
    public void OnRoundEndMove()
    {
        Debug.Log("========================================");
        Debug.Log("Round end stage 1: movement");
        Debug.Log("========================================");
    }

    /// <summary>
    /// This is the second end round step.
    /// At this stage objects may perform their given tasks.
    /// This includes stuff such as gathering resources, damaging other objects, healing, etc.
    /// </summary>
    public void OnRoundEndProcess()
    {
        Debug.Log("========================================");
        Debug.Log("Round end stage 2: processing");
        Debug.Log("========================================");
    }

    /// <summary>
    /// This is the third end round step: dying.
    /// At this stage objects must check whether they have survived the onslaught.
    /// If not, they must do what must be done and remove themselves from play.
    /// </summary>
    public void OnRoundEndDie()
    {
        Debug.Log("========================================");
        Debug.Log("Round end stage 3: dying");
        Debug.Log("========================================");
    }
}
