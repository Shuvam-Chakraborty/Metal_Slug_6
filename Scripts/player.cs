using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;

public class player : MonoBehaviour
{
    //will be used to get the reference of the body which have to be manipulated 
    //public GameObject pl;

    //point of shoot of granade
    public Transform granadePointOfShoot;
    public GameObject granadeSpawner;

    //attack range of a dagger
    public float attackRange;

    public LayerMask enemy_layer;    // Layer of the layer of enemy

    //used for movement 
    private float moveInput;
    //deciding the horizontal and vertical velocity of the player 
    public float h_velocity;
    public float v_velocity;

    //will tell whether player is in crouch
    private bool isincrouch=false;

    //used for storing the reference to the rigid body 2d
    // Reference to the player's Rigidbody2D component
    private CapsuleCollider2D rb;

    private Transform groundlocation;    // Empty GameObject for ground check
    public float radius = 0.2f; // Radius of ground check
    public LayerMask ground;    // Layer of the ground
    public bool onground;// Is the player touching the ground?


    public Transform hPointOfShoot;
    public Transform vPointOfShoot;
    public GameObject horizontalbullets;
    public GameObject verticalbullets;

    public health_script health;
    public int PlayerMaxHealth;

    public GameObject gameover;
    public GameObject background;

    //will tell the player is dead or not, accordingly we will destroy enemy objects
    public bool dead_player;

    //will be used to manipulate uppar and lowe body
    public Animator upperBody;
    public Animator lowerBody;
    public Animator deadplay;
    public Animator crouch;

    //time difference that should be between two consecutive shots \

    public float cooldown;
    private float lastshot=0;

    // Adding audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //setting the position of the player to zero
        transform.position= Vector3.zero;
        //getting the reference 
        rb = GetComponent<CapsuleCollider2D>();
        groundlocation = GetComponent<Transform>();

        health.setMaxHealth(PlayerMaxHealth);

        rb.attachedRigidbody.freezeRotation = true;
        dead_player=false;
        death();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.is_dead())
        {
            shoot();
            dagger();
            posture();
            movements();
            jumps();
            tossgranade();
            //death animation handling
            death();
            
        }
        
        else
        {
            
            death();
            //Destroy(gameObject);
            crouch.SetBool("walkcrouch", false);
            incrouch(false);
            //readjusting scale
            
            groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f, 1);
            background.SetActive(true);
            gameover.SetActive(true);
        }
    }
    

    void tossgranade()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isincrouch)
            {
                upperBody.SetTrigger("granade");
                goto kump;
            }

        crouch.SetTrigger("granade");

        kump:
            Instantiate(granadeSpawner, granadePointOfShoot.position, transform.rotation);
            return;

        }
    }

    void dagger()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlaySFX(audioManager.knife);
            if (!isincrouch)
            {
                upperBody.SetTrigger("dagger");
                goto jump;
            }

            crouch.SetTrigger("dagger");

            jump:
            Vector2 playerPosition = transform.position;
            Vector2 playerDirection = transform.right;  // Assuming the player faces right by default

            // Use Raycasting or Overlap to detect enemies in front of the player
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerPosition, attackRange, enemy_layer);

            foreach (Collider2D enemy in hitEnemies)
            {
                Vector2 directionToEnemy = enemy.transform.position - transform.position;

                // Check if the enemy is in front of the player
                if (Vector2.Dot(playerDirection, directionToEnemy.normalized) > 0)
                {
                    // If the enemy is in front and within range, destroy the enemy
                    GameObject collidedObject = enemy.gameObject;

                    shootingenemies temp = collidedObject.GetComponent<shootingenemies>();
                    temp.damage(200);
                }
            }
        }
    }
    void death()
    {
        
        upperBody.SetBool("isdead", dead_player);
        lowerBody.SetBool("isdead", dead_player);
        deadplay.SetBool("isdead", dead_player);

    }
    void incrouch(bool x)
    {
        isincrouch = x;
        upperBody.SetBool("wantcrouch", x);
        lowerBody.SetBool("wantcrouch", x);
        crouch.SetBool("wantcrouch", x);
        

    }

    void shootTigger()
    {
        if (!isincrouch)
        {
            upperBody.SetTrigger("hshoot");
            return;
        }
        
        crouch.SetTrigger("shoot");
    }
    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.S) && lastshot>=cooldown)
        {
            shootTigger();
            audioManager.PlaySFX(audioManager.player_bullet);
            Instantiate(horizontalbullets,hPointOfShoot.position,transform.rotation);
            lastshot = 0;


        }

        
        
        if (Input.GetKeyDown(KeyCode.W) && lastshot >= cooldown)
        {
            groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f, 1);
            incrouch(false);
            upperBody.SetTrigger("vshoot");
            audioManager.PlaySFX(audioManager.player_bullet);
            Instantiate(verticalbullets, vPointOfShoot.position, transform.rotation);
        }

        lastshot = lastshot + Time.deltaTime;

    }
    void posture()
    {
        if (onground && Input.GetKey(KeyCode.C))
        {
            incrouch(true);
            groundlocation.localScale = new Vector3(groundlocation.localScale.x, 6.5f,1);
        }

        if (onground && Input.GetKey(KeyCode.Space))
        {
            crouch.SetBool("walkcrouch", false);

            incrouch(false);
            groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f,1);
        }
        //if (onground && Input.GetKey(KeyCode.L))
        //{
        //    groundlocation.localScale = new Vector3(groundlocation.localScale.x, 3f,1);
        //}
    }

    void movements()
    {   
        //getting the direction to move in
        moveInput = Input.GetAxisRaw("Horizontal");
        crouch.SetBool("walkcrouch", moveInput != 0f);
        if (moveInput < 0f)
        {
            //player can move only in standing position
            //incrouch(false);
            //groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f, 1);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (moveInput > 0f)
        {
            //incrouch(false);
            //player can move only in standing position
            //groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f, 1);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        upperBody.SetBool("walk", moveInput != 0f);
        lowerBody.SetBool("walk", moveInput != 0f);


        // Update the player's velocity based on the input
        rb.attachedRigidbody.velocity = new Vector2(moveInput * h_velocity, rb.attachedRigidbody.velocity.y);
    }

    void jumps()
    {
        onground = Physics2D.OverlapCircle(new Vector2 (rb.bounds.center.x,rb.bounds.min.y), radius, ground);

        // Jump if the player is grounded and the jump key is pressed
        if (onground && Input.GetKeyDown(KeyCode.Space))
        {
            lowerBody.SetTrigger("jump");
            audioManager.PlaySFX(audioManager.jump);
            rb.attachedRigidbody.velocity = new Vector2(rb.attachedRigidbody.velocity.x, v_velocity);
        }
        

    }

    public void reduce_health(int amount)
    {
        health.weaker(amount);
        if (health.is_dead())
        {
            audioManager.PlaySFX(audioManager.player_death);
            //incrouch(false);
            dead_player = true;
        }
    }
}
