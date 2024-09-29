using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Mathematics;

public class boss : MonoBehaviour
{
    
    //will be used to get the target at which "it" will shoot 
    public GameObject target;

    public GameObject cannons;


    //used for storing the reference to the rigid body 2d
    // Reference to the enemy's Rigidbody2D component
    private PolygonCollider2D rb;

    //access the mass
    private Rigidbody2D mass;

    //deciding the horizontal and vertical velocity of the boss
    public float h_velocity;


    //following parameters will be used to apporach the target during random intervals

    public float minapproachrate;
    public float maxapproachrate;

    private float approachrate;
    private float approachTimer = 0;

    public float maxMovementTime;
    private float moveTimer = 0;


    //object will get destroyed after some time

    public float destroyTimer;
    private float timePassed=0;
    private bool startCounting;

    //getting all the animators related to childrens 
    public Animator cann;
    public Animator mainbody;
    public Animator wheels;
    public Animator tail;


    //amount of score that the player will get if this get shot 
    public int rewardOnBeingShot;

    public Transform pointOfShoot;


    //following parameters will be used to shoot the target during random intervals
    public float minshootrate;
    public float maxshootrate;

    private float shootrate;
    private float shootTimer = 0;

    public float minVerticalVelocityOfCannon;


    public float tobetakenshots;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("mainplayer");
        rb = GetComponent<PolygonCollider2D>();
        mass = GetComponent<Rigidbody2D>();

        transform.position=new Vector3(95f, 3.5f, 0);

        rb.attachedRigidbody.freezeRotation = true;
        //getting the reference 







    }

    // Update is called once per frame
    void Update()
    {
        if (math.abs(target.transform.position.x - transform.position.x) > 20f)
        {
            return;
        }

        if (!startCounting)
        {
            shoot();
            approach();
            
        }
            

        if (tobetakenshots <= 0f && !startCounting)
        {
            cann.SetTrigger("destroyed");
            mainbody.SetTrigger("destroyed");
            wheels.SetTrigger("destroyed");
            tail.SetTrigger("destroyed");
            startCounting = true;


            mass.gravityScale = 0;
            rb.attachedRigidbody.velocity = Vector2.zero;
            rb.enabled = false;
            audioManager.PlaySFX(audioManager.boss1_death);

        }
        if (startCounting)
        {
            timePassed += Time.deltaTime;
            if (timePassed > destroyTimer)
                Destroy(gameObject);
        }
    }

    void adjustapproachrate()
    {
        approachrate = UnityEngine.Random.Range(minapproachrate, maxapproachrate);
    }


    void approach()
    {
        if (approachTimer < approachrate)
        {
            approachTimer = approachTimer + Time.deltaTime;
            return;
        }

        //i don't want the enemies to be too close to me and i also don't want the enemies to chase me if i am too far 
        if (math.abs(target.transform.position.x - transform.position.x) < 7.5)
        {

            
            //asm = false;
            return;



        }

        if (moveTimer < maxMovementTime)
        {
            moveTimer += Time.deltaTime;



            rb.attachedRigidbody.velocity = new Vector2((-1f)*(transform.right.x) * h_velocity, rb.attachedRigidbody.velocity.y);
            wheels.SetBool("ismoving", true);
            //shootOrApproach = walkOrShoot();
            return;
        }

        wheels.SetBool("ismoving",false);

        approachTimer = 0;
        moveTimer = 0;
        adjustapproachrate();

    }

    void adjustshootrate()
    {
        shootrate = UnityEngine.Random.Range(minshootrate, maxshootrate);
    }


    void shoot()
    {
        //shootOrApproach = walkOrShoot();

        //shootOrApproach = walkOrShoot();
        if (shootTimer < shootrate)
        {
            shootTimer = shootTimer + Time.deltaTime;
            return;
        }
        shootTimer = 0;
        adjustshootrate();


        //if enemy is out of the camera, than we don't want it to shoot 
        if (math.abs(target.transform.position.x - transform.position.x) < 17f  /*&& !asm*/ )
        {
            canon temp = cannons.GetComponent<canon>();
            if (temp.vspeed > minVerticalVelocityOfCannon)
                temp.vspeed -= 0.5f;
            else
                temp.vspeed = 6.5f;

            cann.SetTrigger("shoot");
            audioManager.PlaySFX(audioManager.cannon_1);
            Instantiate(cannons, pointOfShoot.position, pointOfShoot.rotation);


        }
        //asm = true;
        //shootOrApproach = walkOrShoot();
    }

    public void damage(float damage)
    {
        tobetakenshots = tobetakenshots - damage;
        
    }
}
