using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granades : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public float hspeed;
    public float vspeed;

    //getting the rigid body of the granade
    public Rigidbody2D rb;

    

    //getting the animator of the grande

    public Animator anim;
    

    //will be used to increment the score when bullet shoots enemy
    public GameObject score;

    //amount of damage that will be caused to enemies 
    public float harm;


    public float explosionRadius;

    //will be used to store the layer mask of the oppnenets in the explosion radius
    public LayerMask enemy;

    public LayerMask boss;
    public LayerMask boss2;
    public LayerMask boss3;

    public bool exploded=false;

    //private bool tocallanim = true;

    public float explosionTime;
    private float timepassed=0;


    public GameObject pl;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {

        pl = GameObject.FindGameObjectWithTag("mainplayer");

        //deadzone = pl.transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2((transform.right.x) * hspeed, vspeed);
        score = GameObject.FindGameObjectWithTag("score");
    }
    private void Update()
    {
        if(exploded)
            
            if (timepassed < explosionTime)
                timepassed += Time.deltaTime;
            else
                Destroy(gameObject); 
        

    }

    //void handleanimations()
    //{
    //    if (exploded)
    //    {


    //    }
    //    tocallanim = false;
    //}

    void OnCollisionEnter2D(Collision2D collision)
    {
        exploded = true;
        anim.SetTrigger("causeexplosion");

        audioManager.PlaySFX(audioManager.grenade);


        //incrementing the score
        
        //granades will cause an increment of 5 in the score 

        Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemy);

        foreach (Collider2D enemy in hitEnemies1)
        {
            GameObject collidedObject = enemy.gameObject;
            shootingenemies temp = collidedObject.GetComponent<shootingenemies>();
            temp.damage(harm);
            score lemp = score.GetComponent<score>();
            lemp.increment_score(1);

            //kill_appropriate(enemy);



        }

        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(transform.position, explosionRadius, boss);

        foreach (Collider2D enemy in hitEnemies2)
        {
            GameObject collidedObject = enemy.gameObject;
            boss temp = collidedObject.GetComponent<boss>();
            temp.damage(harm);
            score lemp = score.GetComponent<score>();
            lemp.increment_score(1);

            //kill_appropriate(enemy);



        }

        Collider2D[] hitEnemies3 = Physics2D.OverlapCircleAll(transform.position, explosionRadius, boss2);

        foreach (Collider2D enemy in hitEnemies3)
        {
            GameObject collidedObject = enemy.gameObject;
            boss2 temp = collidedObject.GetComponent<boss2>();
            temp.damage(harm);
            score lemp = score.GetComponent<score>();
            lemp.increment_score(1);

            //kill_appropriate(enemy);



        }

        Collider2D[] hitEnemies4 = Physics2D.OverlapCircleAll(transform.position, explosionRadius, boss3);

        foreach (Collider2D enemy in hitEnemies4)
        {
            GameObject collidedObject = enemy.gameObject;
            boss3 temp = collidedObject.GetComponent<boss3>();
            temp.damage(harm);
            score lemp = score.GetComponent<score>();
            lemp.increment_score(1);

            //kill_appropriate(enemy);



        }


    }

    //void kill_appropriate(Collider2D collision)
    //{
    //    // Check if the object we collided with has any kind of Collider2D
    //    GameObject collidedObject = collision.gameObject;

    //    int layerIndex = collidedObject.layer;

    //    // Convert the layer index to the layer name
    //    string layerName = LayerMask.LayerToName(layerIndex);
    //    if (layerName == "enemy")
    //    {
    //        shootingenemies temp = collidedObject.GetComponent<shootingenemies>();
    //        int val = temp.rewardOnBeingShot;


    //        score lemp = score.GetComponent<score>();
    //        lemp.increment_score(val);
    //        temp.damage(harm);

    //    }

    //    if (layerName == "boss")
    //    {
    //        boss temp = collidedObject.GetComponent<boss>();
    //        int val = temp.rewardOnBeingShot;


    //        score lemp = score.GetComponent<score>();
    //        lemp.increment_score(val);
    //        temp.damage(harm);

    //    }
    //}






}
