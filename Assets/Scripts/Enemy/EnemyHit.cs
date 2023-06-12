using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public AudioClip deadSFX;
    
    public static int enemyCount = 0;
    
    private LevelManager lm;
    void Start()
    {
        enemyCount++;
        lm = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (LevelManager.isGameOver)
        {
            enemyCount = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Projectile"))
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!LevelManager.isGameOver)
        {
            enemyCount--;
            AudioSource.PlayClipAtPoint(deadSFX, transform.position);
            if (enemyCount <= 0)
            {
                lm.LevelBeat();
            }
        }
    }
}
