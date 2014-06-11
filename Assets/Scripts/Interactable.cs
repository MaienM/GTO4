using UnityEngine;
using System.Collections;

/// <summary>
/// An object with which can be interacted.
/// </summary>
public class Interactable : MonoBehaviour 
{
    /// <summary>
    /// The resources that are associated with this interactable.
    /// </summary>
    public Resources resources = new Resources();

    /// <summary>
    /// How much damage this interactable can sustain before dying.
    /// </summary>
    public float health = 100;

    public void OnRoundEndDie()
    {
        if (health <= 0)
        {
            Debug.Log(string.Format("Death: {0}", ToString()));
            Die();
        }
    }

    /// <summary>
    /// Called when the unit dies. This is responsible for destroying the object.
    /// </summary>
    protected void Die()
    {
        Destroy(gameObject);
    }
}
