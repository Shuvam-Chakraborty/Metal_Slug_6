using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;

using UnityEngine;


public class stabbingenemies : MonoBehaviour
{
    //will be used to get the reference of the body which have to be manipulated 
    //public GameObject pl;

    public int harmed;
    public float attackRange;
    //amount of score that the player will get if this get shot 
    public LayerMask enemy_layer;    // Layer of the layer of enemy
    public int rewardOnBeingShot;
    //will be used to get the direction
    public float diff;

    //deciding the horizontal and vertical velocity of the player 
    public float h_velocity;
    public float v_velocity;

    //my player will either move or stab

    //public bool asm=true;//initially player will only move 



    //used for storing the reference to the rigid body 2d
    // Reference to the player's Rigidbody2D component
    private CapsuleCollider2D rb;


    //access the mass
    private Rigidbody2D mass;

    private Transform groundlocation;    // Empty GameObject for ground check
    public float radius = 0.2f; // Radius of ground check
    public LayerMask ground;    // Layer of the ground
    public bool onground;// Is the player touching the ground?

    
    


    //will be used to get the target at which "it" will stab 
    public GameObject target;


    //following parameters will be used to stab the target during random intervals
    public float minstabrate;
    public float maxstabrate;

    private float stabrate;
    private float stabTimer = 0;



    //public float enemyBulletSpeed;

    //following parameters will be used to apporach the target during random intervals

    

    private Animator enemy_anim;

    

    public float tobetakenshots;

    public float play_death = 0.55f;

    public float tobedestoryed = 0f;


    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //since enemies are created at run time, they have to find target at run time , for that we have used tag
        target = GameObject.FindGameObjectWithTag("mainplayer");

        //getting the reference 

        rb = GetComponent<CapsuleCollider2D>();
        mass = GetComponent<Rigidbody2D>();
        groundlocation = GetComponent<Transform>();

        enemy_anim = GetComponent<Animator>();

        rb.attachedRigidbody.freezeRotation = true;
        adjuststabrate();

        

        //stabOrApproach=false;

    }

    // Update is called once per frame
    void Update()
    {
        //getting the direction to move in, if diff is negative, it means that target is to the left of me
        //else at the right


        stab();


        movement();

        handle_animations();

        // Update the player's velocity based on the input

        //jumps();


        player temp = target.GetComponent<player>();

        if (temp.dead_player)
            Destroy(gameObject);
    }

    //will help in deciding walk or stab

    void handle_animations()
    {
        if (tobetakenshots <= 0)
        {
            if (tobedestoryed < play_death)
            {
                tobedestoryed += Time.deltaTime;
                return;
            }
            else
                Destroy(gameObject);
        }
        

        //showing death

    }
    void movement()
    {
        facing();
        apporach();


    }

    
    void apporach()
    {
        //stabOrApproach = walkOrstab();
        onground = Physics2D.OverlapCircle(new Vector2(rb.bounds.center.x, rb.bounds.min.y), radius, ground);

        if (!onground)
            return;
        

        //i don't want the enemies to be too close to me 
        if (math.abs(target.transform.position.x - transform.position.x) < 1.5f)        {
            rb.attachedRigidbody.velocity = Vector2.zero;
            //if enemy is too close, i want it to stab anyway
            //asm = false;
            return;



        }

        


            rb.attachedRigidbody.velocity = new Vector2((transform.right.x) * h_velocity, rb.attachedRigidbody.velocity.y);
            //stabOrApproach = walkOrstab();
        

        //now i want the enemy to stab
        //asm = false;

        
        //stabOrApproach = walkOrstab();

    }

    void adjuststabrate()
    {
        stabrate = UnityEngine.Random.Range(minstabrate, maxstabrate);
    }
    void stab()
    {
        //stabOrApproach = walkOrstab();
        if (stabTimer < stabrate)
        {
            stabTimer = stabTimer + Time.deltaTime;
            return;
        }
        stabTimer = 0;
        adjuststabrate();

        //if enemy is out of the camera, than we don't want it to stab 
        if (math.abs(target.transform.position.x - transform.position.x) < 2f && rb.attachedRigidbody.velocity.x == 0 /*&& !asm*/ )
        {
            audioManager.PlaySFX(audioManager.knife);
            enemy_anim.SetTrigger("stabed");
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
                    int layerIndex = collidedObject.layer;
                    string layerName = LayerMask.LayerToName(layerIndex);
                    if (layerName == "player")
                    {
                        player temp = collidedObject.GetComponent<player>();
                        temp.reduce_health(harmed);

                    }

                    
                }
            }
            // Check if the object we collided with has any kind of Collider2D
            

            

            // Convert the layer index to the layer name
            
            





        }
        //asm = true;
        //stabOrApproach = walkOrstab();
    }

    void facing()
    {
        diff = transform.position.x - target.transform.position.x;

        if (diff > 0f)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        if (diff < 0f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //will be invoked when "it" gets shot 
    public void damage(float damage)
    {
        tobetakenshots = tobetakenshots - damage;
        if (tobetakenshots <= 0)
        {
            mass.gravityScale = 0;
            rb.attachedRigidbody.velocity = Vector2.zero;
            rb.enabled = false;

            enemy_anim.SetTrigger("isdead");
            //audioManager.PlaySFX(audioManager.enemy_death);

        }
    }


    //void posture()
    //{
    //    if (onground && Input.GetKey(KeyCode.C))
    //    {
    //        groundlocation.localScale = new Vector3(groundlocation.localScale.x, 6.5f, 1);
    //    }

    //    if (onground && Input.GetKey(KeyCode.Space))
    //    {
    //        groundlocation.localScale = new Vector3(groundlocation.localScale.x, 10f, 1);
    //    }
    //    if (onground && Input.GetKey(KeyCode.L))
    //    {
    //        groundlocation.localScale = new Vector3(groundlocation.localScale.x, 3f, 1);
    //    }
    //}







    //void jumps()
    //{
    //    onground = Physics2D.OverlapCircle(new Vector2(rb.bounds.center.x, rb.bounds.min.y), radius, ground);

    //    // Jump if the player is grounded and the jump key is pressed
    //    if (onground && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        rb.attachedRigidbody.velocity = new Vector2(rb.attachedRigidbody.velocity.x, v_velocity);
    //    }
    //}
}
