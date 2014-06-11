using UnityEngine;
using System.Collections;

public class UnitController : Interactable
{
    // The player this unit belongs to.
    public PlayerController player;

    // How fast this uset can gather resources.
    public Resources gathering = new Resources();

    // How much damage this unit can do.
    public float damage = 0;

    // The object the user is currently interacting with.
    public Interactable interactingWith;
	
	new public void Update() 
	{
        base.Update();

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
