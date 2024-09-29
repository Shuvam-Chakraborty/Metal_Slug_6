using UnityEngine;

public class BoxHit : MonoBehaviour
{
    public int hitsToDestroy = 3;  // Number of hits before the box is destroyed
    private int currentHits = 0;   // Current number of hits received

    // Method to call when the box is hit
    public void TakeDamage()
    {
        currentHits++;

        // Check if the box has been hit enough times to be destroyed
        if (currentHits >= hitsToDestroy)
        {
            Destroy(gameObject);  // Destroy the box
        }
    }
}
