using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst.Intrinsics;

public class verticalbullets : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public float deadzone;

    public float harm;

    public GameObject score;

    public GameObject pl;
    private void Start()
    {
        pl = GameObject.FindGameObjectWithTag("mainplayer");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
        score = GameObject.FindGameObjectWithTag("score");
    }
    private void Update()
    {
        if (math.abs(pl.transform.position.y - transform.position.y) > deadzone)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        int layerIndex = collidedObject.layer;

        // Convert the layer index to the layer name
        string layerName = LayerMask.LayerToName(layerIndex);
        if (layerName == "Enemy")
        {
            shootingenemies temp = collidedObject.GetComponent<shootingenemies>();
            int val = temp.rewardOnBeingShot;


            score lemp = score.GetComponent<score>();
            lemp.increment_score(val);
            temp.damage(harm);


        }

        if (collidedObject!=null)
            Destroy(gameObject);
    }
}
