using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour  
{
    /// <summary>
    /// The resources the player has available for building stuff.
    /// </summary>
    public GameResources resources = new GameResources();

    /// <summary>
    /// The home position of this player.
    /// </summary>
    public Unit home;

    public GameObject homePrefab;

    public void Start()
    {
        ConsoleCommandsRepository ccr = ConsoleCommandsRepository.Instance;
        ccr.RegisterCommand("setRes", SetResource);
        ccr.RegisterCommand("setResAll", SetResourceAll);
    }

    public void LevelBuilt(LevelBuilder lb)
    {
        Tile tile = null;
        if (PhotonNetwork.isMasterClient)
        {
            tile = lb.grid[0, 0];
        }
        else
        {
            tile = lb.grid[lb.grid.GetLength(0) - 1, lb.grid.GetLength(1) - 1];
        }

        if (tile.Occupant != null)
        {
            tile.Occupant.photonView.RPC("Destroy", PhotonTargets.AllBuffered);
        }

        GameObject homeObj = PhotonNetwork.Instantiate(homePrefab.name, tile.transform.position, homePrefab.transform.rotation, 0) as GameObject;
        home = homeObj.GetComponent<Unit>();
        home.SetTile(tile);
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

    private void SetResourceAll(params string[] args)
    {
        if (args.Length != 1)
        {
            Debug.Log("Usage: setRes [amount]");
            return;
        }

        float amount;
        if (!float.TryParse(args[0], out amount))
        {
            Debug.Log("Amount must be a number");
            return;
        }

        resources.STONE = amount;
        resources.IRON = amount;
        resources.WOOD = amount;
    }
}
