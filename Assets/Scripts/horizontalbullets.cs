using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst.Intrinsics;


public class horizontalbullets : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Rigidbody2D rb;
    public float deadzone;

    //will be used to increment the score when bullet shoots enemy
    public GameObject score;

    //amount of damage that will be caused to enemies 
    public float harm;

    public GameObject pl;

    
    private void Start()
    {
        pl= GameObject.FindGameObjectWithTag("mainplayer");
        //deadzone = pl.transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2( (transform.right.x) * speed,0);
        score=GameObject.FindGameObjectWithTag("score");
    }
    private void Update()
    {
        if (math.abs(pl.transform.position.x-transform.position.x) > deadzone)
            Destroy(gameObject);

        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with has any kind of Collider2D
        GameObject collidedObject = collision.gameObject;

        int layerIndex = collidedObject.layer;

        // Convert the layer index to the layer name
        string layerName = LayerMask.LayerToName(layerIndex);
        if (layerName=="enemy")
        {
            shootingenemies temp= collidedObject.GetComponent<shootingenemies>();
            int val=temp.rewardOnBeingShot;


            score lemp=score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);

        }

        if (layerName == "stabber")
        {
            stabbingenemies temp = collidedObject.GetComponent<stabbingenemies>();
            int val = temp.rewardOnBeingShot;


            score lemp = score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);

        }

        if (layerName == "boss")
        {
            boss temp = collidedObject.GetComponent<boss>();
            int val = temp.rewardOnBeingShot;


            score lemp = score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);

        }
        if (layerName == "boss3")
        {
            boss3 temp = collidedObject.GetComponent<boss3>();
            int val = temp.rewardOnBeingShot;


            score lemp = score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);

        }
        if (layerName == "boss2")
        {
            boss2 temp = collidedObject.GetComponent<boss2>();
            int val = temp.rewardOnBeingShot;


            score lemp = score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);

        }

        if (collidedObject.CompareTag("Box"))
        {
            BoxHit box = collidedObject.GetComponent<BoxHit>();
            if (box != null)
            {
                box.TakeDamage();  // Apply damage to the box
            }
        }

        if (collidedObject != null)
            Destroy(gameObject);
    }
}
