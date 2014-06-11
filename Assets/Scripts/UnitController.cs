using UnityEngine;
using System.Collections;

/// <summary>
/// An unit that can move around, attack and harvest things.
/// </summary>
public class UnitController : Interactable
{
    /// <summary>
    /// The player this unit belongs to.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// How fast this uset can gather resources.
    /// </summary>
    public Resources gathering = new Resources();

    /// <summary>
    /// How much damage this unit can do.
    /// </summary>
    public float damage = 0;

    /// <summary>
    /// The object the user is currently interacting with.
    /// </summary>
    public Interactable interactingWith;
	
	public void OnTurnEnd() 
	{
        // Interact.
        if (interactingWith != null)
        {
            // Gather resources.
            player.resources += interactingWith.resources * gathering;

            // Damage units.
            interactingWith.health -= damage;
        }
	}
}
