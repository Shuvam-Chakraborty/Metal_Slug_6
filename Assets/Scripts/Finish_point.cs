using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public bool debug = true;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("mainplayer"))
        {
            audioManager.PlaySFX(audioManager.mission_complete);
            debug = false;
            // go to next level
            SceneController.instance.NextLevel();

        }
    }
}
