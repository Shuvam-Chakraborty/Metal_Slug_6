using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class boss2 : MonoBehaviour
{

    //object will get destroyed after some time

    public float destroyTimer;
    private float timePassed = 0;
    private bool startCounting;

    //amount of score that the player will get if this get shot 
    public int rewardOnBeingShot;

    public float tobetakenshots;


    public GameObject cannons;

    public float minVerticalVelocityOfCannon;


    //used for storing the reference to the rigid body 2d
    // Reference to the enemy's Rigidbody2D component
    private PolygonCollider2D rb;

    //access the mass
    private Rigidbody2D mass;

    public Transform pointOfShoot1;
    public Transform pointOfShoot2;

    //following parameters will be used to shoot the target during random intervals
    public float minshootrate;
    public float maxshootrate;

    private float shootrate;
    private float shootTimer = 0;


    //will be used to handle the animations 
    public Animator hermit;

    //will be used to get the target at which "it" will shoot 
    public GameObject target;

    //will be used to get the direction
    public float diff;

    //will decide time after which central cannon will be deployed

    public float centralCannonDeployer;
    private float timePassedForCannonToBeDeployed=0;

    //will tell from which cannon to shoot from
    public bool central=false;


    //following parameters will be used to apporach the target during random intervals

    public float minapproachrate;
    public float maxapproachrate;

    private float approachrate;
    private float approachTimer = 0;

    public float maxMovementTime;
    private float moveTimer = 0;
    //deciding the horizontal and vertical velocity of the boss
    public float h_velocity;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(195f, 3.5f, 0);


        //since enemies are created at run time, they have to find target at run time , for that we have used tag
        target = GameObject.FindGameObjectWithTag("mainplayer");

        rb = GetComponent<PolygonCollider2D>();
        mass = GetComponent<Rigidbody2D>();

        rb.attachedRigidbody.freezeRotation = true;
        //i am disabling the animations of the boss 2 till its in range 

        //hermit.enabled = false;
        //experiment code
    }

    // Update is called once per frame
    void Update()
    {
        //if main player is out of range then stay their
        //if (math.abs(target.transform.position.x - transform.position.x) < 17f)
        //{
        //    transform.position = new Vector3(93.61f, 3.41f, 0);
        //    return;
        //}
        if (math.abs(target.transform.position.x - transform.position.x) > 20f)
        {
            return;
        }
        if (!startCounting)
        {
            facing();
            deployer();
            shoot();

            approach();

        }

        if (tobetakenshots <= 0f && !startCounting)
        {
            hermit.SetTrigger("isdead");
            
            startCounting = true;


            mass.gravityScale = 0;
            rb.attachedRigidbody.velocity = Vector2.zero;
            rb.enabled = false;

            audioManager.PlaySFX(audioManager.boss2_death);
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
        if (math.abs(target.transform.position.x - transform.position.x) < 10f)
        {


            //asm = false;
            return;



        }

        if (moveTimer < maxMovementTime)
        {
            moveTimer += Time.deltaTime;



            rb.attachedRigidbody.velocity = new Vector2( (transform.right.x) * h_velocity, rb.attachedRigidbody.velocity.y);
            
            return;
        }

        

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
        if (shootTimer < shootrate)
        {
            shootTimer = shootTimer + Time.deltaTime;
            return;
        }
        shootTimer = 0;
        adjustshootrate();

        //if enemy is out of the camera, than we don't want it to shoot 
        if (math.abs(target.transform.position.x - transform.position.x) < 17f)
        {
            //gameObject.SetActive(true);
            //above was just an expiriment \

            
           

            canon temp = cannons.GetComponent<canon>();
            if (temp.vspeed > minVerticalVelocityOfCannon)
                temp.vspeed -= 0.5f;
            else
                temp.vspeed = 6.5f;


            hermit.SetTrigger("shoot");
            audioManager.PlaySFX(audioManager.cannon_2);
            if(central)
            Instantiate(cannons, pointOfShoot1.position, pointOfShoot1.rotation);

            else
                Instantiate(cannons, pointOfShoot2.position, pointOfShoot2.rotation);
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

    void deployer()
    {
        timePassedForCannonToBeDeployed += Time.deltaTime;
        if (timePassedForCannonToBeDeployed >= centralCannonDeployer)
        {
            timePassedForCannonToBeDeployed = 0f;
            central = !central;
            if (central)
            {
                hermit.SetBool("incannon", central);
                hermit.SetBool("deployedcannon", central);
            }

            else
            {
                hermit.SetBool("deployedcannon", central);
                hermit.SetBool("incannon", central);
                
            }
            

        }

    }
    public void damage(float damage)
    {
        tobetakenshots = tobetakenshots - damage;

    }
}
