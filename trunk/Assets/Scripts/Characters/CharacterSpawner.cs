using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour
{
    public float spawnTime = 5f;		// Amount of time before each spawn
    public float spawnDelay = 5f;		// Time before spawning starts
    public GameObject[] citizens;		// Array of character prefabs
    private static bool spawnFlag = false;
    
    void Start()
    {
        //Start calling the Spawn function repeatedly after a delay .
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }


    void Spawn()
    {
        Debug.Log("Tower is saying: " + Tower.canAddCit().ToString());
        Debug.Log("spawnFlag is saying: " + spawnFlag.ToString());
        
        //If a citizen can be spawned.
        if ((Tower.canAddCit() == true) && (spawnFlag == false))
        {
            spawnFlag = true;
            //Instantiate a random character type (M/F Adult/Child)
            int characterIndex = Random.Range(0, citizens.Length);
            Instantiate(citizens[characterIndex], transform.position, transform.rotation);
            spawnFlag = false;
        }
    }
}