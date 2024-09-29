using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canon : MonoBehaviour
{
    public float hspeed;
    public float vspeed;
    public Rigidbody2D rb;
    //public float deadzone;

    public int damage;


    //getting the animator of the grande

    public Animator anim;

    //public health_script health;

    public bool exploded = false;

    //private bool tocallanim = true;

    public float explosionTime;
    private float timepassed = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2((transform.right.x) * hspeed, vspeed);
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (exploded)

            if (timepassed < explosionTime)
                timepassed += Time.deltaTime;
            else
                Destroy(gameObject);


    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        rb.velocity = Vector2.zero;
        // Check if the object we collided with has any kind of Collider2D
        GameObject collidedObject = collision.gameObject;

        int layerIndex = collidedObject.layer;

        // Convert the layer index to the layer name
        string layerName = LayerMask.LayerToName(layerIndex);
        if (layerName == "player")
        {
            
            anim.SetTrigger("hitplayer");
            player temp = collidedObject.GetComponent<player>();
            temp.reduce_health(damage);

        }

        else
        {
            //it was changed intentionally, animation when player is hit looks more natural
            anim.SetTrigger("hitplayer");
        }
        exploded = true;
    }

}
