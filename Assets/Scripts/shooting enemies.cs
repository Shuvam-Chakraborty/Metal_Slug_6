using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;

public class shootingenemies : MonoBehaviour
{
    //will be used to get the reference of the body which have to be manipulated 
    //public GameObject pl;


    //amount of score that the player will get if this get shot 
    public int rewardOnBeingShot;
    //will be used to get the direction
    public float diff;

    //deciding the horizontal and vertical velocity of the player 
    public float h_velocity;
    public float v_velocity;

    //my player will either move or shoot

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

    public Transform pointofshoot;
    public GameObject bullets;


    //will be used to get the target at which "it" will shoot 
    public GameObject target;


    //following parameters will be used to shoot the target during random intervals
    public float minshootrate;
    public float maxshootrate;

    private float shootrate;
    private float shootTimer=0;

    

    //public float enemyBulletSpeed;

    //following parameters will be used to apporach the target during random intervals

    public float minapproachrate;
    public float maxapproachrate;

    private float approachrate;
    private float approachTimer = 0;
    //public bool willApproach=false;

    private Animator enemy_anim;

    public float maxMovementTime;
    private float moveTimer = 0;

    public float tobetakenshots;

    public float play_death=0.55f;

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
        mass= GetComponent<Rigidbody2D>();
        groundlocation = GetComponent<Transform>();

        enemy_anim=GetComponent<Animator>();

        rb.attachedRigidbody.freezeRotation = true;
        adjustshootrate();

        adjustapproachrate();

        //shootOrApproach=false;

    }

    // Update is called once per frame
    void Update()
    {
        //getting the direction to move in, if diff is negative, it means that target is to the left of me
        //else at the right

        
        shoot();

        
        movement();

        handle_animations();

        // Update the player's velocity based on the input

        //jumps();


        player temp = target.GetComponent<player>();

        if (temp.dead_player)
            Destroy(gameObject);
    }

    //will help in deciding walk or shoot

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
        if(rb.attachedRigidbody.velocity.x!=0)

            enemy_anim.SetBool("isapproaching",true);
        else
            enemy_anim.SetBool("isapproaching", false);

        //showing death

    }
    void movement()
    {
        facing();
        apporach();
        

    }

    void adjustapproachrate()
    {
        approachrate = UnityEngine.Random.Range(minapproachrate, maxapproachrate);
    }
    void apporach()
    {
        //shootOrApproach = walkOrShoot();
        onground = Physics2D.OverlapCircle(new Vector2(rb.bounds.center.x, rb.bounds.min.y), radius, ground);

        if (!onground)
            return;
        if (approachTimer < approachrate)
        {
            approachTimer = approachTimer + Time.deltaTime;
            return;
        }


        //i don't want the enemies to be too close to me 
        if (math.abs(target.transform.position.x - transform.position.x) < 4)
        {

            //if enemy is too close, i want it to shoot anyway
            //asm = false;
            return;


           
        }

        if ( moveTimer<maxMovementTime )
        {
            moveTimer += Time.deltaTime;

            
            
            rb.attachedRigidbody.velocity = new Vector2((transform.right.x) * h_velocity, rb.attachedRigidbody.velocity.y);
            //shootOrApproach = walkOrShoot();
            return;
        }

        //now i want the enemy to shoot
        //asm = false;
        
        approachTimer = 0;
        moveTimer = 0;
        adjustapproachrate();
        //shootOrApproach = walkOrShoot();

    }

    void adjustshootrate()
    {
        shootrate = UnityEngine.Random.Range(minshootrate, maxshootrate);
    }
    void shoot()
    {
        //shootOrApproach = walkOrShoot();
        if (shootTimer < shootrate)
        {
            shootTimer=shootTimer+Time.deltaTime;
            return;
        }
        shootTimer = 0;
        adjustshootrate();

        //if enemy is out of the camera, than we don't want it to shoot 
        if (math.abs(target.transform.position.x - transform.position.x) < 8.75f && rb.attachedRigidbody.velocity.x==0 /*&& !asm*/ )
        {
            enemy_anim.SetTrigger("shoot");
            audioManager.PlaySFX(audioManager.enemy_bullet);
            Instantiate(bullets, pointofshoot.position, transform.rotation);
            

        }
        //asm = true;
        //shootOrApproach = walkOrShoot();
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
            rb.attachedRigidbody.velocity=Vector2.zero;
            rb.enabled = false;
            
            enemy_anim.SetTrigger("isdead");
            audioManager.PlaySFX(audioManager.enemy_death);
            
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
