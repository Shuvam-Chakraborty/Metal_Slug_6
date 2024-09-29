using UnityEngine;

public class HeartHit : MonoBehaviour
{
    public int healthIncreaseAmount = 20;  // Amount of health to increase when picked up

    // Make sure the heart's collider is set as a trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the heart (player has the tag "mainplayer")
        if (collision.CompareTag("mainplayer"))
        {
            // Access the player's health script and increase health
            player playerScript = collision.GetComponent<player>();

            if (playerScript != null)
            {
                playerScript.health.increase_health(healthIncreaseAmount);  // Increase player's health
            }

            // Destroy the heart after being picked up
            Destroy(gameObject);
        }
    }
}
