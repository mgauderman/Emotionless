using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
	
    public float spawnTime = 3f;
    private Vector3 spawnPosition;
    private bool playerDead = false;
 
    // Use this for initialization
    void Start()
    {
        
        InvokeRepeating("Spawn", spawnTime, spawnTime);
 
    }

    void Update()
    {
        if (playerDead)
            CancelInvoke();
    }

    void Spawn()
    {
        if (Random.value < 0.5f)
            spawnPosition.x = Random.Range(-9f, -7f);
        else
            spawnPosition.x = Random.Range(7f, 9f);
        if (Random.value < 0.5f)
            spawnPosition.y = Random.Range(-5f, -3f);
        else
            spawnPosition.y = Random.Range(3f, 5f);

        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

    void PlayerDead()
    {
        playerDead = true;
    }
}
