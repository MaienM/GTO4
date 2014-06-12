using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    /// <summary>
    /// The resources the player has available for building stuff.
    /// </summary>
    public GameResources resources = new GameResources();

    public void Start()
    {
        ConsoleCommandsRepository ccr = ConsoleCommandsRepository.Instance;
        ccr.RegisterCommand("setRes", SetResource);
    }

    private void SetResource(params string[] args)
    {
        if (args.Length != 2)
        {
            Debug.Log("Usage: setRes [resource] [amount]");
            return;
        }

        float amount;
        if (!float.TryParse(args[1], out amount))
        {
            Debug.Log("Amount must be a number");
            return;
        }

        switch (args[0])
        {
            case "STONE":
                resources.STONE = amount;
                break;

            case "IRON":
                resources.IRON = amount;
                break;

            case "WOOD":
                resources.WOOD = amount;
                break;

            default:
                Debug.Log("Resource must be one of: STONE, IRON, WOOD");
                return;
        }
    }
}
