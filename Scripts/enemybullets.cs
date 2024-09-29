using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst.Intrinsics;

public class enemybullets : MonoBehaviour
{// Start is called before the first frame update
    public float speed;
    public Rigidbody2D rb;
    public float deadzone;

    public int damage;

    //public health_script health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2((transform.right.x) * speed, 0);
    }
    private void Update()
    {   
        if (math.abs(transform.position.x) > deadzone)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with has any kind of Collider2D
        GameObject collidedObject = collision.gameObject;

        int layerIndex = collidedObject.layer;

        // Convert the layer index to the layer name
        string layerName = LayerMask.LayerToName(layerIndex);
        if (layerName == "player")
        {
            player temp = collidedObject.GetComponent<player>();
            temp.reduce_health(damage) ;

        }

        if (collidedObject != null)
            Destroy(gameObject);
    }
}
