using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonspawner : MonoBehaviour
{
    public GameObject enemies;
    
    

    public float minspawnrate;
    public float maxspawnrate;

    private float spawnrate;
    private float spawnTimer = 0;

    public player pl;

    //storing the reference of the right wall

    public GameObject rightwall;




    // Start is called before the first frame update
    void Start()
    {
        spawnerPosition();

        adjustspawnrate();
        //Instantiate(enemies, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        spawnerPosition();

        //enemies should not be spawnedoutside the wall
        if (!should_spawn())
            return;
        if (!pl.dead_player)
            spawn();
    }

    bool should_spawn()
    {
        if (gameObject.transform.position.x > rightwall.transform.position.x - 2f)
            return false;
        return true;
    }
    void spawnerPosition()
    {
        //enemies will get spawned from some specified distance 
        if (pl.transform.position.x >= 0)
        {
            gameObject.transform.position = new Vector3(pl.transform.position.x, 7f, 0);
            return;
        }

        gameObject.transform.position = new Vector3(pl.transform.position.x , 7f, 0);
    }
    void adjustspawnrate()
    {
        spawnrate = Random.Range(minspawnrate, maxspawnrate);
    }
    void spawn()
    {
        if (spawnTimer < spawnrate)
        {
            spawnTimer = spawnTimer + Time.deltaTime;
            return;
        }
        spawnTimer = 0;
        adjustspawnrate();

        float temp = Random.Range(-1,1.1f);
        Instantiate(enemies, new Vector3( transform.position.x+temp,transform.position.y,transform.position.y), transform.rotation);
            
    }
}
