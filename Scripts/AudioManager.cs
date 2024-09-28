using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip boss1_death;
    public AudioClip boss2_death;
    public AudioClip boss3_death;
    public AudioClip cannon_1;
    public AudioClip cannon_2;
    public AudioClip cannon_3;
    public AudioClip enemy_bullet;
    public AudioClip enemy_death;
    public AudioClip grenade;
    public AudioClip jump;
    public AudioClip knife;
    public AudioClip player_bullet;
    public AudioClip player_death;
    public AudioClip start;
    public AudioClip mission_complete;
    public AudioClip theme;

    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        musicSource.clip = theme;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
