using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;

    public GameObject wall;
    //public bool PlayerSpawned = true;
    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateLevel()
    {
        for (int x = 0; x <= width; x += 2)
        {
            for (int y = 0; y <= height; y += 2)
            {
                // Should we place a wall?
                if(x == width || x == 0 || y == 0 || y == height)
                {
                    Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                    Instantiate(wall, pos, Quaternion.identity, transform);
                }
                else if (UnityEngine.Random.value > .68f)
                {
                    // Spawn a wall
                    Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                    Instantiate(wall, pos, Quaternion.identity, transform);
                }
                //else if (!playerSpawned) // Should we spawn a player?
                //{
                //    // Spawn the player
                //    Vector3 pos = new Vector3(x - width / 2f, 1.25f, y - height / 2f);
                //    Instantiate(player, pos, Quaternion.identity);
                //    playerSpawned = true;
                //}
            }
        }
    }
}
