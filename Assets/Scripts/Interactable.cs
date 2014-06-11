using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour 
{
    // The resources that are associated with this interactable.
    public Resources resources = new Resources();

    // How much damage this interactable can sustain before dying.
    public float health = 100;

    public void Update() 
	{
        if (health < 0)
        {
            Die();
        }
	}

    protected void Die()
    {
        Destroy(gameObject);
    }
}
