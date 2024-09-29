using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    
    //will be used to get the location of the player 
    public GameObject pl;

    //will store the reference to right wall

    public GameObject rightWall;


    void Start()
    {
        transform.position = new Vector3 (pl.transform.position.x,transform.position.y,-10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (pl.transform.position.x <= 0)
            return;
        if (pl.transform.position.x >= rightWall.transform.position.x-9f)
            return;
        transform.position = new Vector3(pl.transform.position.x, transform.position.y, -10f);
    }
}
