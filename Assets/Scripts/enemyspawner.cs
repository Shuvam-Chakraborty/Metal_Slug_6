using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyspawner : MonoBehaviour
{

    public GameObject enemies;
    public GameObject enemies2;
    public int starter_enemeies=0;

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
        if(!pl.dead_player)
            spawn();
    }

    bool should_spawn()
    {
        if(gameObject.transform.position.x>rightwall.transform.position.x - 20f)
            return false;
        return true;
    }
    void spawnerPosition()
    {
        //enemies will get spawned from some specified distance 
        if (pl.transform.position.x >= 0)
        {
            gameObject.transform.position = new Vector3(pl.transform.position.x + 15f, 7f, 0);
            return;
        }

        gameObject.transform.position = new Vector3(pl.transform.position.x + 20f, 7f, 0);
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
        if (starter_enemeies == 0)
        {
            Instantiate(enemies, transform.position, transform.rotation);
            starter_enemeies = 1;
        }
        else if (starter_enemeies == 1)
        {
            Instantiate(enemies2, transform.position, transform.rotation);
            starter_enemeies = 0;
        }
    }
}
