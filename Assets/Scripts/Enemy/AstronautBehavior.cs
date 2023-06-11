using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautBehavior : MonoBehaviour
{
    public static int keyEnemyCount = 0;
    public Transform player;
    
    private bool playerDetected;
    private LevelManager lm;
    
    void Start()
    {
        keyEnemyCount++;
        playerDetected = false;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        lm = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver)
        {
            keyEnemyCount = 0;
        }
        if (playerDetected)
        {
            Vector3 directionToTarget = (player.position - transform.parent.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            lookRotation.x = 0;

            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, lookRotation, 10 * Time.deltaTime);
        }
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            playerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited");
            playerDetected = false;
        }
    }

    private void OnDestroy()
    {
        if (!LevelManager.isGameOver)
        {
            keyEnemyCount--;
            if (keyEnemyCount <= 0)
            {
                lm.LevelBeat();
            }
        }
    }
}